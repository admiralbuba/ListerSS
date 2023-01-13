using AutoMapper;
using ListerSS.Models;
using ListerSS.Models.Response;

namespace ListerSS.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageResponse, HubMessage>()
                .ReverseMap();
        }
    }
}
