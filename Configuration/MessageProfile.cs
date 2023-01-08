using AutoMapper;
using ListerSS.Dto;
using ListerSS.Models;

namespace ListerSS.Configuration
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageDto, HubMessage>();
            CreateMap<HubMessage, MessageDto>();
        }

    }
}
