using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class SkillProfile:Profile
    {
        public SkillProfile()
        {
          CreateMap<Skill,SkillViewModel>().ReverseMap();
          CreateMap<Skill,CreateOrEditSkillViewModel>().ReverseMap();
        }
    }
}
