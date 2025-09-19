using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerLogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.CustomerLogos
{
     public record GetCustomerLogosForIndexPageQuery():IRequest<List<CustomerLogoListViewModel>>;
    public class GetCustomerLogosForIndexPageQueryHandler
        : IRequestHandler<GetCustomerLogosForIndexPageQuery, List<CustomerLogoListViewModel>>
    {
        private readonly ICustomerLogoRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetCustomerLogosForIndexPageQueryHandler(
            ICustomerLogoRepository repository,
            IMapper mapper,ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<List<CustomerLogoListViewModel>>
            Handle(GetCustomerLogosForIndexPageQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "customerlogos:index:all";
            var cachedData = await _cacheService.GetAsync<List<CustomerLogoListViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var logos = await _repository.GetAllOrderedAsync(cancellationToken);
            var mapped = _mapper.Map<List<CustomerLogoListViewModel>>(logos);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;

        }
    }

}
