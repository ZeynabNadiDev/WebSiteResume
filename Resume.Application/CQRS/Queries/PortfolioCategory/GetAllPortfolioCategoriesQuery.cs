using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Portfolio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.PortfolioCategories
{
    public record GetAllPortfolioCategoriesQuery() : IRequest<List<PortfolioCategoryViewModel>>;

    public class GetAllPortfolioCategoriesQueryHandler
        : IRequestHandler<GetAllPortfolioCategoriesQuery, List<PortfolioCategoryViewModel>>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllPortfolioCategoriesQueryHandler(IPortfolioCategoryRepository portfolioCategoryRepository,
            IMapper mapper,ICacheService cacheService)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<PortfolioCategoryViewModel>> Handle
            (GetAllPortfolioCategoriesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "portfoliocategories:index:all";
            var cachedData = await _cacheService.GetAsync<List<PortfolioCategoryViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var categories = await _portfolioCategoryRepository.GetAllOrderedAsync(cancellationToken);
            var mapped = _mapper.Map<List<PortfolioCategoryViewModel>>(categories);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }
}
