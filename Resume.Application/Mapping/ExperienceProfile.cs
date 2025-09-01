using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Experience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class ExperienceProfile:Profile
    {
        public ExperienceProfile()
        {
            CreateMap<Experience, ExperienceViewModel>().ReverseMap();
        }
    }
}
