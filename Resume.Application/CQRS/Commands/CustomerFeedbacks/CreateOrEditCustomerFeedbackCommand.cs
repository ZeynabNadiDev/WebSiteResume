using AutoMapper;
using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.CustomerFeedback;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.CustomerFeedbacks
{
    public record CreateOrEditCustomerFeedbackCommand(CreateOrEditCustomerFeedbackViewModel Model)
        :IRequest<bool>;
    public class CreateOrEditCustomerFeedbackCommandHandler : IRequestHandler<CreateOrEditCustomerFeedbackCommand, bool>
    {
        private readonly ICustomerFeedbackRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateOrEditCustomerFeedbackCommandHandler(
            ICustomerFeedbackRepository repository,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _repository = repository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateOrEditCustomerFeedbackCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            if (model.Id == 0)
            {
                var newCustomerFeedback = _mapper.Map<CustomerFeedback>(model);
                await _repository.AddAsync(newCustomerFeedback, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                return true;
            }

            var currentCustomerFeedback = await _repository.GetByIdAsync(model.Id, cancellationToken);

            if (currentCustomerFeedback == null)
                return false;

            _mapper.Map(model, currentCustomerFeedback);
            _repository.Update(currentCustomerFeedback);
            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}
