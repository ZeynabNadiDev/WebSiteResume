using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerLogo;

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
        private readonly IMapper _mapper;

        public CustomerLogoService(ICustomerLogoRepository customerLogoRepository,IMapper mapper)
        {
            _customerLogoRepository = customerLogoRepository;
            _mapper = mapper;
        }

        #endregion


        public async Task<List<CustomerLogoListViewModel>> GetCustomerLogosForIndexPage(CancellationToken cancellationToken)
        {
            var customerLogos = await _customerLogoRepository.GetAllOrderedAsync(cancellationToken);


            return _mapper.Map<List<CustomerLogoListViewModel>>(customerLogos);


        }


    }
}
