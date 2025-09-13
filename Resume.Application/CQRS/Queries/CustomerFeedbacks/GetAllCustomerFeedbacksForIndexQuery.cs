using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.CustomerFeedbacks
{
    public record GetAllCustomerFeedbacksForIndexQuery():IRequest<List<CustomerFeedbackViewModel>>;
    public class GetAllCustomerFeedbacksForIndexQueryHandler : IRequestHandler<GetAllCustomerFeedbacksForIndexQuery, List<CustomerFeedbackViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerFeedbackRepository _repository;

        public GetAllCustomerFeedbacksForIndexQueryHandler(IMapper mapper, ICustomerFeedbackRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CustomerFeedbackViewModel>> Handle(GetAllCustomerFeedbacksForIndexQuery request, CancellationToken cancellationToken)
        {
            var customerFeedbacks = await _repository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<CustomerFeedbackViewModel>>(customerFeedbacks);
        }

    }
}
