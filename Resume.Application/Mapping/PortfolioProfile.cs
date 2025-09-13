using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Portfolio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class PortfolioProfile:Profile
    {
        public PortfolioProfile()
        {
          CreateMap<Portfolio,PortfolioViewModel>().ReverseMap();
          CreateMap<PortfolioCategory, PortfolioCategoryViewModel>().ReverseMap();
          CreateMap<Portfolio,CreateOrEditPortfolioViewModel>().ReverseMap();
          CreateMap<PortfolioCategory, CreateOrEditPortfolioCategoryViewModel>().ReverseMap();
          
        }
    }
}
