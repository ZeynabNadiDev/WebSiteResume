using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Portfolios
{
    public record DeletePortfolioCommand(long Id) : IRequest<bool>;

    public class DeletePortfolioCommandHandler
        : IRequestHandler<DeletePortfolioCommand, bool>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeletePortfolioCommandHandler> _logger;

        public DeletePortfolioCommandHandler(
            IPortfolioRepository portfolioRepository,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<DeletePortfolioCommandHandler> logger)
        {
            _portfolioRepository = portfolioRepository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeletePortfolioCommand, Id: {Id}", request.Id);

            try
            {
                var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);
                if (portfolio == null)
                {
                    _logger.LogWarning("Portfolio not found for Id: {Id}", request.Id);
                    return false;
                }

                _portfolioRepository.Delete(portfolio);
                await _uow.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Portfolio deleted successfully, Id: {Id}", request.Id);

                // Cache invalidation
                await _cacheService.RemoveAsync($"portfolio:{request.Id}:entity");
                await _cacheService.RemoveAsync("portfolios:index:all");
                _logger.LogInformation("Cache invalidated for portfolio:{Id}:entity and portfolios:index:all", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Portfolio, Id: {Id}", request.Id);
                throw;
            }
        }
    }
}
