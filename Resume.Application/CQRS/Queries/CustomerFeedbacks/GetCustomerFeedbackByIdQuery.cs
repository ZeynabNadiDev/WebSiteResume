using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.CustomerFeedbacks
{
    public record GetCustomerFeedbackByIdQuery(long Id) : IRequest<CustomerFeedback>;
    public class GetCustomerFeedbackByIdQueryHandler : IRequestHandler<GetCustomerFeedbackByIdQuery, CustomerFeedback>
    {
        private readonly ICustomerFeedbackRepository _repository;
        public GetCustomerFeedbackByIdQueryHandler(ICustomerFeedbackRepository repository)
        {
            _repository = repository;
        }
        public async Task<CustomerFeedback> Handle(GetCustomerFeedbackByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }

}
