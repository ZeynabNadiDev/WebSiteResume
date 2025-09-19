using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Educations
{
    public record DeleteEducationCommand(long Id) : IRequest<bool>;

    public class DeleteEducationHandler : IRequestHandler<DeleteEducationCommand, bool>
    {
        private readonly IEducationRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeleteEducationHandler> _logger;

        public DeleteEducationHandler(
            IEducationRepository repo,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<DeleteEducationHandler> logger)
        {
            _repo = repo;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteEducationCommand, Id: {Id}", request.Id);

            try
            {
                var education = await _repo.GetByIdAsync(request.Id, cancellationToken);
                if (education == null)
                {
                    _logger.LogWarning("Education not found for Id: {Id}", request.Id);
                    return false;
                }

                _repo.Delete(education);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Deleted Education successfully, Id: {Id}", request.Id);

                // Cache invalidation
                await _cacheService.RemoveAsync($"education:{request.Id}:entity");
                await _cacheService.RemoveAsync("educations:index:all");

                _logger.LogInformation("Cache invalidated for Education Id: {Id}", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Education, Id: {Id}", request.Id);
                throw;
            }
        }
    }
}
