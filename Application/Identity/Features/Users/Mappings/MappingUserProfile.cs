using AutoMapper;
using SuperMarket.Domain.Identity;
using SuperMarket.Shared.Requests.Identity;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Mappings
{
    internal class MappingUserProfile : Profile
    {
        public MappingUserProfile()
        {
            CreateMap<UserRegistrationRequest, ApplicationUser>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ActivateUser))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.AutoComfirmEmail));

            CreateMap<UpdateUserRequest, ApplicationUser>();

            CreateMap<ApplicationUser, UserResponse>();
        }
    }
}