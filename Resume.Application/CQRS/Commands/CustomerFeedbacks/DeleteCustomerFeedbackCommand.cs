using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.CustomerFeedbacks
{
    public record DeleteCustomerFeedbackCommand(long Id):IRequest<bool>;
    public class DeleteCustomerFeedbackCommandHandler : IRequestHandler<DeleteCustomerFeedbackCommand, bool>
    {
        private readonly ICustomerFeedbackRepository _repository;
        private readonly IUnitOfWork _uow;

        public DeleteCustomerFeedbackCommandHandler(ICustomerFeedbackRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteCustomerFeedbackCommand request, CancellationToken cancellationToken)
        {
            var customerFeedback = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (customerFeedback == null) return false;

            _repository.Delete(customerFeedback);
            await _uow.SaveChangesAsync(cancellationToken);

            return true;

        }
    }

}
