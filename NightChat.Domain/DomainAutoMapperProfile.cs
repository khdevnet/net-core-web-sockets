using AutoMapper;
using NightChat.Domain.Dto;
using NightChat.Domain.Entities;


namespace NightChat.Domain
{
    public class DomainAutoMapperProfile : Profile
    {
        public DomainAutoMapperProfile()
        {
            CreateMap<Token, TokenData>()
                .ForCtorParam("accessToken", opts => opts.MapFrom(src => src.Value));

            CreateMap<TokenData, Token>()
                .ForMember(dest => dest.Value, opts => opts.MapFrom(src => src.AccessToken));

            CreateMap<UserData, User>()
                .ForMember(dest => dest.Url, opts => opts.MapFrom(src => src.Avatar));

            CreateMap<User, UserData>()
                .ForMember(dest => dest.Avatar, opts => opts.MapFrom(src => src.Url));
        }
    }
}