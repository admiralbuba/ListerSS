using AutoMapper;
using Lister.Domain.Models;
using Lister.WebApi.Models.Response;

namespace Lister.WebApi.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageResponse, HubMessage>()
                .ReverseMap();
            CreateMap<Group, CreateGroupResponse>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(x => x.Guid).ToList()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Guid))
                .ReverseMap();
        }
    }
}
