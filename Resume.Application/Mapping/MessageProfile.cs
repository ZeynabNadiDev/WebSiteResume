using AutoMapper;
using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Mapping
{
    public class MessageProfile:Profile
    {
        public MessageProfile() 
        {
            CreateMap<Message, MessageViewModel>().ReverseMap();
            CreateMap<Message, CreateMessageViewModel>().ReverseMap();
        }
    }
}
