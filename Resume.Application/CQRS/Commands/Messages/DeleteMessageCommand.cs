using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Messages
{
    public record DeleteMessageCommand(long Id):IRequest<bool>;
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
    {
        private readonly IMessageRepository _repository;
        private readonly IUnitOfWork _uow;

        public DeleteMessageCommandHandler(IMessageRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }
        public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (message == null) return false;
            _repository.Delete(message);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
