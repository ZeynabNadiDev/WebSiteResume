using AutoMapper;
using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Messages
{
    public record CreateMessageCommand(CreateMessageViewModel message) :IRequest<bool>;
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, bool>
    {
        private readonly IMessageRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateMessageCommandHandler(IMessageRepository repository, IMapper mapper, IUnitOfWork uow)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<bool> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var newMessage = _mapper.Map<Message>(request.message);
            await _repository.AddAsync(newMessage, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
