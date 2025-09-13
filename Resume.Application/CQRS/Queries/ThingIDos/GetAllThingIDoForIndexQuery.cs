using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.ThingIDo;
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

        public GetAllThingIDoForIndexQueryHandler(IThingIDoRepository thingIDoRepository, IMapper mapper)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
        }

        public async Task<List<ThingIDoListViewModel>> Handle(GetAllThingIDoForIndexQuery request, CancellationToken cancellationToken)
        {
            var thingIDos = await _thingIDoRepository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<ThingIDoListViewModel>>(thingIDos);
        }
    }
}
