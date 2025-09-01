using AutoMapper;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.ViewModels.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class ReservationProfile:Profile
    {
        public ReservationProfile() 
        {
            CreateMap<ReservationDate, CreateOrUpdateReservationViewModel>()
            .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")))
            .ReverseMap()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.ReservationDate)));
        }
    }
}
