using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.PortfolioCategories
{
    public record DeletePortfolioCategoryCommand(long Id) : IRequest<bool>;

    public class DeletePortfolioCategoryCommandHandler
        : IRequestHandler<DeletePortfolioCategoryCommand, bool>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeletePortfolioCategoryCommandHandler> _logger;

        public DeletePortfolioCategoryCommandHandler(
            IPortfolioCategoryRepository portfolioCategoryRepository,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<DeletePortfolioCategoryCommandHandler> logger)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePortfolioCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeletePortfolioCategoryCommand, Id: {Id}", request.Id);

            try
            {
                var category = await _portfolioCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
                if (category == null)
                {
                    _logger.LogWarning("PortfolioCategory not found for Id: {Id}", request.Id);
                    return false;
                }

                _portfolioCategoryRepository.Delete(category);
                await _uow.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("PortfolioCategory deleted successfully, Id: {Id}", request.Id);

                await _cacheService.RemoveAsync($"portfoliocategory:{request.Id}:entity");
                await _cacheService.RemoveAsync("portfoliocategories:index:all");
                _logger.LogInformation("Cache invalidated for portfoliocategory:{Id}:entity and portfoliocategories:index:all", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting PortfolioCategory, Id: {Id}", request.Id);
                throw;
            }
        }
    }
}
