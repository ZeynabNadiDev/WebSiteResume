using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Information;
using Resume.Domain.Entity;
using System.Threading;
using System.Threading.Tasks;
using Resume.Application.Redis.Caching.Interfaces;

namespace Resume.Application.CQRS.Commands.Informations
{
    public record CreateOrEditInformationCommand(CreateOrEditInformationViewModel Model) :IRequest<bool>;
    public class CreateOrEditInformationCommandHandler
    {
        private readonly IInformationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        public CreateOrEditInformationCommandHandler(
            IInformationRepository repository,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
            _cacheService=cacheService;
        }

        public async Task<bool> Handle(CreateOrEditInformationCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            if (model.Id == 0)
            {
                var newEntity = _mapper.Map<Information>(model);
                await _repository.AddAsync(newEntity, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                await _cacheService.RemoveAsync("information:entity");

                return true;

            }
            var currentEntity = await _repository.GetSingleAsync(cancellationToken);
            if (currentEntity == null)
                return false;

            _mapper.Map(model, currentEntity);
            _repository.Update(currentEntity);
            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync("information:entity");

            return true;
        }
    }

}
