using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Messages
{
    public record DeleteMessageCommand(long Id) : IRequest<bool>;

    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
    {
        private readonly IMessageRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeleteMessageCommandHandler> _logger;

        public DeleteMessageCommandHandler(
            IMessageRepository repository,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<DeleteMessageCommandHandler> logger)
        {
            _repository = repository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteMessageCommand, Id: {Id}", request.Id);

            try
            {
                var message = await _repository.GetByIdAsync(request.Id, cancellationToken);
                if (message == null)
                {
                    _logger.LogWarning("Message not found for Id: {Id}", request.Id);
                    return false;
                }

                _repository.Delete(message);
                await _uow.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Deleted Message successfully, Id: {Id}", request.Id);

                // Cache Invalidation
                await _cacheService.RemoveAsync($"message:{request.Id}:entity");
                await _cacheService.RemoveAsync("messages:index:all");
                _logger.LogInformation("Cache invalidated for message:{Id}:entity and messages:index:all", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Message, Id: {Id}", request.Id);
                throw;
            }
        }
    }
}
