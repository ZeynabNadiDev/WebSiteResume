using AutoMapper;
using MediatR;
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

        public CreateOrEditThingIDoCommandHandler(IThingIDoRepository thingIDoRepository, IMapper mapper, IUnitOfWork uow)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> Handle(CreateOrEditThingIDoCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.Id == 0)
            {
                var newEntity = _mapper.Map<ThingIDo>(request.Model);
                await _thingIDoRepository.AddAsync(newEntity, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                return true;
            }

            var entity = await _thingIDoRepository.GetByIdAsync(request.Model.Id, cancellationToken);
            if (entity == null) return false;

            _mapper.Map(request.Model, entity);
            _thingIDoRepository.Update(entity);

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
