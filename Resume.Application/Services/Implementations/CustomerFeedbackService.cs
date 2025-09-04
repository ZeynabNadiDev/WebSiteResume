using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerFeedback;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class CustomerFeedbackService : ICustomerFeedbackService
    {

        #region Constructor ICustomerFeedbackRepository
        private readonly ICustomerFeedbackRepository _customerFeedbackRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CustomerFeedbackService(ICustomerFeedbackRepository customerFeedbackRepository,IMapper mapper,IUnitOfWork unit)
        {
            _customerFeedbackRepository= customerFeedbackRepository;
            _mapper= mapper;
            _uow= unit;

        }

        #endregion

        public async Task<CustomerFeedback> GetCustomerFeedbackByIdAsync(long id,CancellationToken cancellationToken)
        {
            return await _customerFeedbackRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<CustomerFeedbackViewModel>> GetAllCustomerFeedbacksAsyncForIndex(CancellationToken cancellationToken)
        {
            var customerFeedbacks = await _customerFeedbackRepository.GetAllOrderedAsync(cancellationToken);


            return _mapper.Map<List<CustomerFeedbackViewModel>>(customerFeedbacks);
        }

        public async Task<bool> CreateOrEditCustomerFeedbackAsync(CreateOrEditCustomerFeedbackViewModel customerFeedback,CancellationToken cancellationToken)
        {
            if (customerFeedback.Id == 0)
            {
                var newCustomerFeedback = _mapper.Map<CustomerFeedback>(customerFeedback);

                await _customerFeedbackRepository.AddAsync(newCustomerFeedback,cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                return true;
            }


            CustomerFeedback currentCustomerFeedback = await GetCustomerFeedbackByIdAsync(customerFeedback.Id,cancellationToken);

            if (currentCustomerFeedback == null) return false;

            _mapper.Map(customerFeedback, currentCustomerFeedback);

            _customerFeedbackRepository.Update(currentCustomerFeedback);
            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<CreateOrEditCustomerFeedbackViewModel> FillCreateOrEditCustomerFeedbackViewModelAsync(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditCustomerFeedbackViewModel() {Id = 0 };

            CustomerFeedback customerFeedback = await GetCustomerFeedbackByIdAsync(id, cancellationToken);

            if (customerFeedback == null) return new CreateOrEditCustomerFeedbackViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditCustomerFeedbackViewModel>(customerFeedback);

        }

        public async Task<bool> DeleteCustomerFeedbackAsync(long id, CancellationToken cancellationToken)
        {
            CustomerFeedback customerFeedback = await GetCustomerFeedbackByIdAsync(id,cancellationToken);

            if (customerFeedback == null) return false;

            _customerFeedbackRepository.Delete(customerFeedback);
            await _uow.SaveChangesAsync(cancellationToken) ;

            return true;
        }


    }
}
