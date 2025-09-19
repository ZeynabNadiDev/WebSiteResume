using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.PortfolioCategories
{
    public record GetPortfolioCategoryByIdQuery(long Id) : IRequest<PortfolioCategory?>;

    public class GetPortfolioCategoryByIdQueryHandler
        : IRequestHandler<GetPortfolioCategoryByIdQuery, PortfolioCategory?>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly ICacheService _cacheService;

        public GetPortfolioCategoryByIdQueryHandler(IPortfolioCategoryRepository portfolioCategoryRepository,ICacheService cacheService)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _cacheService = cacheService;
        }

        public async Task<PortfolioCategory?> Handle
            (GetPortfolioCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var cacheKey = $"portfoliocategory:{request.Id}:entity";
            var cachedData = await _cacheService.GetAsync<PortfolioCategory>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var entity = await _portfolioCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity != null)
                await _cacheService.SetAsync(cacheKey, entity, TimeSpan.FromMinutes(10));

            return entity;

        }
    }
}
