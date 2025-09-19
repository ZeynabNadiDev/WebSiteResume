using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.ThingIDo;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.ThingIDos
{
    public record CreateOrEditThingIDoCommand(CreateOrEditThingIDoViewModel Model) : IRequest<bool>;

    public class CreateOrEditThingIDoCommandHandler
        : IRequestHandler<CreateOrEditThingIDoCommand, bool>
    {
        private readonly IThingIDoRepository _thingIDoRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public CreateOrEditThingIDoCommandHandler(IThingIDoRepository thingIDoRepository,
            IMapper mapper, IUnitOfWork uow,ICacheService cacheService)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(CreateOrEditThingIDoCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.Id == 0)
            {
                var newEntity = _mapper.Map<ThingIDo>(request.Model);
                await _thingIDoRepository.AddAsync(newEntity, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                await _cacheService.RemoveAsync("thingidos:index:all");

                return true;
            }

            var entity = await _thingIDoRepository.GetByIdAsync(request.Model.Id, cancellationToken);
            if (entity == null) return false;

            _mapper.Map(request.Model, entity);
            _thingIDoRepository.Update(entity);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"thingido:{request.Model.Id}:entity");
            await _cacheService.RemoveAsync("thingidos:index:all");

            return true;
        }
    }
}
