using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerFeedback;
using Resume.Infra.Data.Context;
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

        public CustomerFeedbackService(ICustomerFeedbackRepository customerFeedbackRepository)
        {
            _customerFeedbackRepository= customerFeedbackRepository;
        }

        #endregion

        public async Task<CustomerFeedback> GetCustomerFeedbackByIdAsync(long id,CancellationToken cancellationToken)
        {
            return await _customerFeedbackRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<CustomerFeedbackViewModel>> GetAllCustomerFeedbacksAsyncForIndex(CancellationToken cancellationToken)
        {
            var customerFeedbacks = await _customerFeedbackRepository.GetAllOrderedAsync(cancellationToken);


               return customerFeedbacks.Select(c => new CustomerFeedbackViewModel()
                {
                    Order = c.Order,
                    Avatar = c.Avatar,
                    Description = c.Description,
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        public async Task<bool> CreateOrEditCustomerFeedbackAsync(CreateOrEditCustomerFeedbackViewModel customerFeedback,CancellationToken cancellationToken)
        {
            if (customerFeedback.Id == 0)
            {
                var newCustomerFeedback = new CustomerFeedback()
                {
                    Avatar = customerFeedback.Avatar,
                    Description = customerFeedback.Description,
                    Name = customerFeedback.Name,
                    Order = customerFeedback.Order
                };

                await _customerFeedbackRepository.AddAsync(newCustomerFeedback,cancellationToken);
                await _customerFeedbackRepository.SaveChangeAsync(cancellationToken);

                return true;
            }


            CustomerFeedback currentCustomerFeedback = await GetCustomerFeedbackByIdAsync(customerFeedback.Id,cancellationToken);

            if (currentCustomerFeedback == null) return false;

            currentCustomerFeedback.Avatar = customerFeedback.Avatar;
            currentCustomerFeedback.Description = customerFeedback.Description;
            currentCustomerFeedback.Name = customerFeedback.Name;
            currentCustomerFeedback.Order = customerFeedback.Order;

            _customerFeedbackRepository.Update(currentCustomerFeedback);
            await _customerFeedbackRepository.SaveChangeAsync(cancellationToken);

            return true;
        }

        public async Task<CreateOrEditCustomerFeedbackViewModel> FillCreateOrEditCustomerFeedbackViewModelAsync(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditCustomerFeedbackViewModel() {Id = 0 };

            CustomerFeedback customerFeedback = await GetCustomerFeedbackByIdAsync(id, cancellationToken);

            if (customerFeedback == null) return new CreateOrEditCustomerFeedbackViewModel() { Id = 0 };

            return new CreateOrEditCustomerFeedbackViewModel()
            {
                Id = customerFeedback.Id,
                Avatar = customerFeedback.Avatar,
                Description = customerFeedback.Description,
                Name = customerFeedback.Name,
                Order = customerFeedback.Order
            };

        }

        public async Task<bool> DeleteCustomerFeedbackAsync(long id, CancellationToken cancellationToken)
        {
            CustomerFeedback customerFeedback = await GetCustomerFeedbackByIdAsync(id,cancellationToken);

            if (customerFeedback == null) return false;

            _customerFeedbackRepository.Delete(customerFeedback);
            await _customerFeedbackRepository.SaveChangeAsync(cancellationToken) ;

            return true;
        }


    }
}
