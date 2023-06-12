using AutoMapper;
using Lister.Domain.Models;
using Lister.WebApi.Models.Response;
using Lister.WebApi.SignalR;

namespace Lister.WebApi.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, HubMessage>()
                .ReverseMap();
            CreateMap<Group, CreateGroup>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(x => x.Guid).ToList()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Guid))
                .ReverseMap();
        }
    }
}
