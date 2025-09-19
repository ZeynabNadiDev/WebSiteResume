using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.ThingIDo;
using System;
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
        private readonly ILogger<CreateOrEditThingIDoCommandHandler> _logger;

        public CreateOrEditThingIDoCommandHandler(
            IThingIDoRepository thingIDoRepository,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateOrEditThingIDoCommandHandler> logger)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditThingIDoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrEditThingIDoCommand, Id: {Id}", request.Model.Id);

            try
            {
                if (request.Model.Id == 0) // Create
                {
                    _logger.LogInformation("Creating new ThingIDo with Title: {Title}", request.Model.Title);

                    var newEntity = _mapper.Map<ThingIDo>(request.Model);
                    await _thingIDoRepository.AddAsync(newEntity, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("ThingIDo created successfully with generated Id: {Id}", newEntity.Id);

                    await _cacheService.RemoveAsync("thingidos:index:all");
                    _logger.LogInformation("Cache invalidated: thingidos:index:all");

                    return true;
                }

                _logger.LogInformation("Editing existing ThingIDo, Id: {Id}", request.Model.Id);

                var entity = await _thingIDoRepository.GetByIdAsync(request.Model.Id, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning("ThingIDo not found for Id: {Id}", request.Model.Id);
                    return false;
                }

                _mapper.Map(request.Model, entity);
                _thingIDoRepository.Update(entity);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ThingIDo updated successfully, Id: {Id}", request.Model.Id);

                await _cacheService.RemoveAsync($"thingido:{request.Model.Id}:entity");
                await _cacheService.RemoveAsync("thingidos:index:all");
                _logger.LogInformation("Cache invalidated for thingido:{Id}:entity and thingidos:index:all", request.Model.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling CreateOrEditThingIDoCommand, Id: {Id}", request.Model.Id);
                throw;
            }
        }
    }
}
