using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Skills
{
    public record DeleteSkillCommand(long Id) : IRequest<bool>;

    public class DeleteSkillHandler : IRequestHandler<DeleteSkillCommand, bool>
    {
        private readonly ISkillRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeleteSkillHandler> _logger;

        public DeleteSkillHandler(
            ISkillRepository repo,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<DeleteSkillHandler> logger)
        {
            _repo = repo;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteSkillCommand, SkillId: {Id}", request.Id);

            try
            {
                var skill = await _repo.GetByIdAsync(request.Id, cancellationToken);
                if (skill == null)
                {
                    _logger.LogWarning("Skill not found for Id: {Id}", request.Id);
                    return false;
                }

                _repo.Delete(skill);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Skill deleted successfully, Id: {Id}", request.Id);

                await _cacheService.RemoveAsync($"skill:{request.Id}:entity");
                await _cacheService.RemoveAsync("skills:index:all");
                _logger.LogInformation("Cache invalidated for skill:{Id}:entity and skills:index:all", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Skill, Id: {Id}", request.Id);
                throw;
            }
        }
    }
}
