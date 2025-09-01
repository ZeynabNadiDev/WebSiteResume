using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.CustomerLogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class CustomerLogoProfile:Profile
    {
        public CustomerLogoProfile()
        {
            CreateMap<CustomerLogo, CustomerLogoListViewModel>().ReverseMap();
        }
    }
}
