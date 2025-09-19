using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Portfolios
{
    public record GetPortfolioByIdQuery(long Id) : IRequest<Portfolio?>;

    public class GetPortfolioByIdQueryHandler
        : IRequestHandler<GetPortfolioByIdQuery, Portfolio?>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ICacheService _cacheService;

        public GetPortfolioByIdQueryHandler(IPortfolioRepository portfolioRepository,ICacheService cacheService)
        {
            _portfolioRepository = portfolioRepository;
            _cacheService = cacheService;  
        }

        public async Task<Portfolio?> Handle(GetPortfolioByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var cacheKey = $"portfolio:{request.Id}:entity";
            var cachedData = await _cacheService.GetAsync<Portfolio>(cacheKey);
            if (cachedData != null)
                return cachedData;
            var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);
            if (portfolio != null)
                await _cacheService.SetAsync(cacheKey, portfolio, TimeSpan.FromMinutes(10));

             return portfolio;
        }
    }

}
