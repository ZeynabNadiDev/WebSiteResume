using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.ThingIDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class ThingToDoProfile:Profile
    {
        public ThingToDoProfile() 
        {
            CreateMap<ThingIDo,ThingIDoListViewModel>().ReverseMap();
            CreateMap<ThingIDo,CreateOrEditThingIDoViewModel>().ReverseMap();
        }
    }
}
