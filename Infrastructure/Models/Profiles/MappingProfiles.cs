using AutoMapper;
using SuperMarket.Common.Responses.Identity;

namespace SuperMarket.Infrastructure.Models.Mappings
{
    internal class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationRole, RoleResponse>();
            CreateMap<ApplicationRoleClaim, RoleClaimViewModel>().ReverseMap();
        }
    }
}