using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.ThingIDo;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.ThingIDos
{
    public record GetAllThingIDoForIndexQuery() : IRequest<List<ThingIDoListViewModel>>;

    public class GetAllThingIDoForIndexQueryHandler
        : IRequestHandler<GetAllThingIDoForIndexQuery, List<ThingIDoListViewModel>>
    {
        private readonly IThingIDoRepository _thingIDoRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllThingIDoForIndexQueryHandler(IThingIDoRepository thingIDoRepository, 
            IMapper mapper,ICacheService cacheService)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
            _cacheService = cacheService;

        }

        public async Task<List<ThingIDoListViewModel>> Handle
            (GetAllThingIDoForIndexQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "thingidos:index:all";
            var cachedData = await _cacheService.GetAsync<List<ThingIDoListViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var thingIDos = await _thingIDoRepository.GetAllOrderedAsync(cancellationToken);
            var mapped = _mapper.Map<List<ThingIDoListViewModel>>(thingIDos);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));
            return mapped;
        }
    }
}
