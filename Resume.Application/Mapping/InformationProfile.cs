using AutoMapper;

using Resume.Domain.ViewModels.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resume.Domain.Entity;

namespace Resume.Application.Mapping
{
    public class InformationProfile:Profile
    {
        public InformationProfile() 
        {
            CreateMap<Information,InformationViewModel>().ReverseMap();
            CreateMap<Information,CreateOrEditInformationViewModel>().ReverseMap();
        }
    }
}
