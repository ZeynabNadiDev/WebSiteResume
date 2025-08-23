using Resume.Domain.Entity;
using Resume.Domain.ViewModels.CustomerFeedback;
using Resume.Domain.ViewModels.Skill;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface ICustomerFeedbackService
    {

        Task<CustomerFeedback> GetCustomerFeedbackByIdAsync(long id, CancellationToken cancellationToken);
        Task<List<CustomerFeedbackViewModel>> GetAllCustomerFeedbacksAsyncForIndex(CancellationToken cancellationToken);
        Task<CreateOrEditCustomerFeedbackViewModel> FillCreateOrEditCustomerFeedbackViewModelAsync(long id, CancellationToken cancellationToken);
        Task<bool> CreateOrEditCustomerFeedbackAsync(CreateOrEditCustomerFeedbackViewModel customerFeedbackl, CancellationToken cancellationToken);
        Task<bool> DeleteCustomerFeedbackAsync(long id, CancellationToken cancellationToken);

    }
}
