using Resume.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Domain.Repository
{
    public interface ICustomerFeedbackRepository: IGenericRepository<CustomerFeedback>
    {
        Task<List<CustomerFeedback>> GetAllOrderedAsync(CancellationToken cancellationToken);
    }
}
