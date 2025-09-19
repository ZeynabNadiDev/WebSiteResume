using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
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
        private readonly ICacheService _cacheService;


        public DeleteThingIDoCommandHandler(IThingIDoRepository thingIDoRepository,
            IUnitOfWork uow,ICacheService cacheService)
        {
            _thingIDoRepository = thingIDoRepository;
            _uow = uow;
            _cacheService = cacheService;   
        }

        public async Task<bool> Handle(DeleteThingIDoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _thingIDoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return false;

            _thingIDoRepository.Delete(entity);
            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"thingido:{request.Id}:entity");
            await _cacheService.RemoveAsync("thingidos:index:all");

            return true;
        }
    }
}
