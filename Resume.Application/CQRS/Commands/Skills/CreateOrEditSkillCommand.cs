using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Skill;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Skills
{
    public record CreateOrEditSkillCommand(CreateOrEditSkillViewModel Skill) : IRequest<bool>;

    public class CreateOrEditSkillHandler : IRequestHandler<CreateOrEditSkillCommand, bool>
    {
        private readonly ISkillRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateOrEditSkillHandler> _logger;

        public CreateOrEditSkillHandler(
            ISkillRepository repo,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateOrEditSkillHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditSkillCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrEditSkillCommand, SkillId: {Id}", request.Skill.Id);

            try
            {
                if (request.Skill.Id == 0) // Create
                {
                    _logger.LogInformation("Creating new Skill with Name: {Name}", request.Skill.Title);

                    var newSkill = _mapper.Map<Skill>(request.Skill);
                    await _repo.AddAsync(newSkill, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("Skill created successfully with generated Id: {Id}", newSkill.Id);

                    await _cacheService.RemoveAsync("skills:index:all");
                    _logger.LogInformation("Cache invalidated: skills:index:all");

                    return true;
                }
                else // Update
                {
                    _logger.LogInformation("Editing existing Skill, SkillId: {Id}", request.Skill.Id);

                    var existingSkill = await _repo.GetByIdAsync(request.Skill.Id, cancellationToken);
                    if (existingSkill == null)
                    {
                        _logger.LogWarning("Skill not found for Id: {Id}", request.Skill.Id);
                        return false;
                    }

                    _mapper.Map(request.Skill, existingSkill);
                    _repo.Update(existingSkill);
                    await _uow.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("Skill updated successfully, Id: {Id}", request.Skill.Id);

                    await _cacheService.RemoveAsync($"skill:{request.Skill.Id}:entity");
                    await _cacheService.RemoveAsync("skills:index:all");
                    _logger.LogInformation("Cache invalidated for skill:{Id}:entity and skills:index:all", request.Skill.Id);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling CreateOrEditSkillCommand, SkillId: {Id}", request.Skill.Id);
                throw;
            }
        }
    }
}
