using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Portfolio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.PortfolioCategories
{
    public record FillCreateOrEditPortfolioCategoryViewModelQuery(long Id) : IRequest<CreateOrEditPortfolioCategoryViewModel>;

    public class FillCreateOrEditPortfolioCategoryViewModelQueryHandler
        : IRequestHandler<FillCreateOrEditPortfolioCategoryViewModelQuery, CreateOrEditPortfolioCategoryViewModel>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FillCreateOrEditPortfolioCategoryViewModelQueryHandler(IPortfolioCategoryRepository portfolioCategoryRepository,
            IMapper mapper,ICacheService cacheService)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<CreateOrEditPortfolioCategoryViewModel> Handle
            (FillCreateOrEditPortfolioCategoryViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0) return new CreateOrEditPortfolioCategoryViewModel() { Id = 0 };
            var cacheKey = $"portfoliocategory:{request.Id}";

            var cachedData = await _cacheService.GetAsync<CreateOrEditPortfolioCategoryViewModel>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var category = await _portfolioCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null) return new CreateOrEditPortfolioCategoryViewModel() { Id = 0 };

            var mapped = _mapper.Map<CreateOrEditPortfolioCategoryViewModel>(category);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }
}
