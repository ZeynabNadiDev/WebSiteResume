using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Information;
using Resume.Domain.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Resume.Application.Redis.Caching.Interfaces;

namespace Resume.Application.CQRS.Commands.Informations
{
    public record CreateOrEditInformationCommand(CreateOrEditInformationViewModel Model) : IRequest<bool>;

    public class CreateOrEditInformationCommandHandler : IRequestHandler<CreateOrEditInformationCommand, bool>
    {
        private readonly IInformationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateOrEditInformationCommandHandler> _logger;

        public CreateOrEditInformationCommandHandler(
            IInformationRepository repository,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateOrEditInformationCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditInformationCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            _logger.LogInformation("Handling CreateOrEditInformationCommand, Id: {Id}", model.Id);

            try
            {
                if (model.Id == 0)
                {
                    _logger.LogInformation("Creating new Information record");

                    var newEntity = _mapper.Map<Information>(model);
                    await _repository.AddAsync(newEntity, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("Created Information entity successfully, New Id: {Id}", newEntity.Id);

                    await _cacheService.RemoveAsync("information:entity");
                    _logger.LogInformation("Cache invalidated for information:entity");

                    return true;
                }

                _logger.LogInformation("Editing Information record");

                var currentEntity = await _repository.GetSingleAsync(cancellationToken);
                if (currentEntity == null)
                {
                    _logger.LogWarning("Information entity not found for edit");
                    return false;
                }

                _mapper.Map(model, currentEntity);
                _repository.Update(currentEntity);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Updated Information entity successfully");

                await _cacheService.RemoveAsync("information:entity");
                _logger.LogInformation("Cache invalidated for information:entity");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling CreateOrEditInformationCommand, Id: {Id}", model.Id);
                throw;
            }
        }
    }
}
