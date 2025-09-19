using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
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
        private readonly ILogger<DeleteThingIDoCommandHandler> _logger;

        public DeleteThingIDoCommandHandler(
            IThingIDoRepository thingIDoRepository,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<DeleteThingIDoCommandHandler> logger)
        {
            _thingIDoRepository = thingIDoRepository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteThingIDoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteThingIDoCommand, Id: {Id}", request.Id);

            try
            {
                var entity = await _thingIDoRepository.GetByIdAsync(request.Id, cancellationToken);
                if (entity == null)
                {
                    _logger.LogWarning("ThingIDo not found for Id: {Id}", request.Id);
                    return false;
                }

                _thingIDoRepository.Delete(entity);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ThingIDo deleted successfully, Id: {Id}", request.Id);

                await _cacheService.RemoveAsync($"thingido:{request.Id}:entity");
                await _cacheService.RemoveAsync("thingidos:index:all");
                _logger.LogInformation("Cache invalidated for thingido:{Id}:entity and thingidos:index:all", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Log