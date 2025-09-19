using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.ThingIDos
{
    public record GetThingIDoByIdQuery(long Id) : IRequest<ThingIDo?>;

    public class GetThingIDoByIdQueryHandler
        : IRequestHandler<GetThingIDoByIdQuery, ThingIDo?>
    {
        private readonly IThingIDoRepository _thingIDoRepository;
        private readonly ICacheService _cacheService;

        public GetThingIDoByIdQueryHandler(IThingIDoRepository thingIDoRepository,ICacheService cacheService)
        {
            _thingIDoRepository = thingIDoRepository;
            _cacheService = cacheService;
        }

        public async Task<ThingIDo?> Handle(GetThingIDoByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"thingido:{request.Id}:entity";
            var cachedData = await _cacheService.GetAsync<ThingIDo>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var entity = await _thingIDoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity != null)
                await _cacheService.SetAsync(cacheKey, entity, TimeSpan.FromMinutes(10));

            return entity;
        }
    }
}
