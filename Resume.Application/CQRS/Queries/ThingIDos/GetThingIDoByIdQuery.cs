using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.ThingIDos
{
    public record GetThingIDoByIdQuery(long Id) : IRequest<ThingIDo?>;

    public class GetThingIDoByIdQueryHandler
        : IRequestHandler<GetThingIDoByIdQuery, ThingIDo?>
    {
        private readonly IThingIDoRepository _thingIDoRepository;

        public GetThingIDoByIdQueryHandler(IThingIDoRepository thingIDoRepository)
        {
            _thingIDoRepository = thingIDoRepository;
        }

        public Task<ThingIDo?> Handle(GetThingIDoByIdQuery request, CancellationToken cancellationToken)
        {
            return _thingIDoRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
