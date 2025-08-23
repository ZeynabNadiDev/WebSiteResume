using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerLogo;
using Resume.Infra.Data.Context;
using Resume.Infra.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class CustomerLogoService : ICustomerLogoService
    {

        #region Constructor ICustomerLogoRepository
        private readonly ICustomerLogoRepository _customerLogoRepository;

        public CustomerLogoService(ICustomerLogoRepository customerLogoRepository)
        {
            _customerLogoRepository = customerLogoRepository;
        }

        #endregion


        public async Task<List<CustomerLogoListViewModel>> GetCustomerLogosForIndexPage(CancellationToken cancellationToken)
        {
            var customerLogos = await _customerLogoRepository.GetAllOrderedAsync(cancellationToken);


             return customerLogos.Select(c => new CustomerLogoListViewModel()
                {
                    Id = c.Id,
                    Link = c.Link,
                    Logo = c.Logo,
                    LogoAlt = c.LogoAlt,
                    Order = c.Order
                })
                .ToList();

            
        }


    }
}
