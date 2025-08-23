using Resume.Domain.ViewModels.CustomerLogo;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface ICustomerLogoService
    {
        Task<List<CustomerLogoListViewModel>> GetCustomerLogosForIndexPage(CancellationToken cancellationToken);
    }
}
