using AutoMapper;
using MediatR;
using Resume.Domain.ViewModels.ThingIDo;
using Resume.Domain.Repository;
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

        public FillCreateOrEditThingIDoViewModelQueryHandler(IThingIDoRepository thingIDoRepository, IMapper mapper)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
        }

        public async Task<CreateOrEditThingIDoViewModel> Handle(FillCreateOrEditThingIDoViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditThingIDoViewModel { Id = 0 };

            var entity = await _thingIDoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return new CreateOrEditThingIDoViewModel { Id = 0 };

            return _mapper.Map<CreateOrEditThingIDoViewModel>(entity);
        }
    }
}
