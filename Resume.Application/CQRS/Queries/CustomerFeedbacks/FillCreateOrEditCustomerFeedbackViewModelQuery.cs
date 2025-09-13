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
    public record FillCreateOrEditCustomerFeedbackViewModelQuery(long Id):IRequest<CreateOrEditCustomerFeedbackViewModel>;
    public class FillCreateOrEditCustomerFeedbackViewModelQueryHandler : IRequestHandler<FillCreateOrEditCustomerFeedbackViewModelQuery, CreateOrEditCustomerFeedbackViewModel>
    {
        private readonly ICustomerFeedbackRepository _repository;
        private readonly IMapper _mapper;

        public FillCreateOrEditCustomerFeedbackViewModelQueryHandler(ICustomerFeedbackRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateOrEditCustomerFeedbackViewModel> Handle(FillCreateOrEditCustomerFeedbackViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditCustomerFeedbackViewModel { Id = 0 };

            var customerFeedback = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (customerFeedback == null)
                return new CreateOrEditCustomerFeedbackViewModel { Id = 0 };

            return _mapper.Map<CreateOrEditCustomerFeedbackViewModel>(customerFeedback);
        }
    }

}
