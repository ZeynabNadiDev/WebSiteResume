using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Message;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Messages
{
    public record CreateMessageCommand(CreateMessageViewModel message) : IRequest<bool>;

    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, bool>
    {
        private readonly IMessageRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateMessageCommandHandler> _logger;

        public CreateMessageCommandHandler(
            IMessageRepository repository,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateMessageCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateMessageCommand");

            try
            {
                var newMessage = _mapper.Map<Message>(request.message);
                await _repository.AddAsync(newMessage, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Message created successfully, Id: {Id}", newMessage.Id);

                await _cacheService.RemoveAsync("messages:index:all");
                _logger.LogInformation("Cache invalidated for messages:index:all");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating message");
                throw;
            }
        }
    }
}
