using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.ThingIDo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.ThingIDos
{
    public record FillCreateOrEditThingIDoViewModelQuery(long Id) : IRequest<CreateOrEditThingIDoViewModel>;

    public class FillCreateOrEditThingIDoViewModelQueryHandler
        : IRequestHandler<FillCreateOrEditThingIDoViewModelQuery, CreateOrEditThingIDoViewModel>
    {
        private readonly IThingIDoRepository _thingIDoRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FillCreateOrEditThingIDoViewModelQueryHandler(IThingIDoRepository thingIDoRepository,
            IMapper mapper,ICacheService cacheService)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
            _cacheService = cacheService;

        }

        public async Task<CreateOrEditThingIDoViewModel> Handle
            (FillCreateOrEditThingIDoViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditThingIDoViewModel { Id = 0 };
           
            var cacheKey = $"thingido:{request.Id}";
            var cachedData = await _cacheService.GetAsync<CreateOrEditThingIDoViewModel>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var entity = await _thingIDoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return new CreateOrEditThingIDoViewModel { Id = 0 };

            var mapped = _mapper.Map<CreateOrEditThingIDoViewModel>(entity);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;


        }
    }
}
