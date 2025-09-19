using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Portfolio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Portfolios
{
    public record GetAllPortfoliosQuery() : IRequest<List<PortfolioViewModel>>;
    public class GetAllPortfoliosQueryHandler
       : IRequestHandler<GetAllPortfoliosQuery, List<PortfolioViewModel>>
    {
        private readonly IPortfolioRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllPortfoliosQueryHandler(IPortfolioRepository repository, 
            IMapper mapper,ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<List<PortfolioViewModel>> Handle
            (GetAllPortfoliosQuery request,CancellationToken cancellationToken)
        {
            const string cacheKey = "portfolios:index:all";
            var cachedData = await _cacheService.GetAsync<List<PortfolioViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var portfolios =await _repository.GetAllOrderedAsync(cancellationToken);
            var mapped= _mapper.Map<List<PortfolioViewModel>>(portfolios);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }

    }
}
