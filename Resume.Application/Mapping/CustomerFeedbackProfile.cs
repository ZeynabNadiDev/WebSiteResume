using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.CustomerFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class CustomerFeedbackProfile: Profile
    {
        public CustomerFeedbackProfile() 
        {
            //Main Map
            CreateMap<CustomerFeedback,CustomerFeedbackViewModel>().ReverseMap();

            //Create Or Edit
            CreateMap<CustomerFeedback, CreateOrEditCustomerFeedbackViewModel>().ReverseMap();
        }
    }
}
