using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Education;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Educations
{
    public record CreateOrEditEducationCommand(CreateOrEditEducationViewModel Education) : IRequest<bool>;

    public class CreateOrEditEducationHandler : IRequestHandler<CreateOrEditEducationCommand, bool>
    {
        private readonly IEducationRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateOrEditEducationHandler> _logger;

        public CreateOrEditEducationHandler(
            IEducationRepository repo,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateOrEditEducationHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditEducationCommand request, CancellationToken cancellationToken)
        {
            var eduId = request.Education?.Id ?? 0;
            _logger.LogInformation("Handling CreateOrEditEducationCommand, Id: {Id}", eduId);

            try
            {
                if (eduId == 0)
                {
                    _logger.LogInformation("Creating new Education entry");

                    var newEducation = _mapper.Map<Education>(request.Education);
                    await _repo.AddAsync(newEducation, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    await _cacheService.RemoveAsync("educations:index:all");
                    _logger.LogInformation("Education created successfully, New Id: {Id}", newEducation.Id);

                    return true;
                }
                else
                {
                    _logger.LogInformation("Editing existing Education, Id: {Id}", eduId);

                    var existingEducation = await _repo.GetByIdAsync(eduId, cancellationToken);
                    if (existingEducation == null)
                    {
                        _logger.LogWarning("Education not found for Id: {Id}", eduId);
                        return false;
                    }

                    _mapper.Map(request.Education, existingEducation);
                    _repo.Update(existingEducation);
                    await _uow.SaveChangesAsync(cancellationToken);

                    await _cacheService.RemoveAsync($"education:{eduId}:entity");
                    await _cacheService.RemoveAsync("educations:index:all");

                    _logger.LogInformation("Education updated successfully, Id: {Id}", eduId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling Education command, Id: {Id}", eduId);
                throw;
            }
        }
    }
}
