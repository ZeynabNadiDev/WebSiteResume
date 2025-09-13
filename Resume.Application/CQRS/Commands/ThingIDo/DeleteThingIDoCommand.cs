using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.ThingIDos
{
    public record DeleteThingIDoCommand(long Id) : IRequest<bool>;

    public class DeleteThingIDoCommandHandler
        : IRequestHandler<DeleteThingIDoCommand, bool>
    {
        private readonly IThingIDoRepository _thingIDoRepository;
        private readonly IUnitOfWork _uow;

        public DeleteThingIDoCommandHandler(IThingIDoRepository thingIDoRepository, IUnitOfWork uow)
        {
            _thingIDoRepository = thingIDoRepository;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteThingIDoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _thingIDoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return false;

            _thingIDoRepository.Delete(entity);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
