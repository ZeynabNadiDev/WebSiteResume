using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class EducationProfile:Profile
    {
        public EducationProfile()
        {
            CreateMap<Education,EducationViewModel>().ReverseMap();
            CreateMap<Education, CreateOrEditEducationViewModel>().ReverseMap();
        }
    }
}
