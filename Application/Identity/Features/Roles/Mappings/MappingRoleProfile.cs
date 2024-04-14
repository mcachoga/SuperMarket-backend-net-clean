using AutoMapper;
using SuperMarket.Domain.Identity;
using SuperMarket.Shared.Requests.Identity;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Roles.Mappings
{
    internal class MappingRoleProfile : Profile
    {
        public MappingRoleProfile()
        {
            CreateMap<ApplicationRole, RoleResponse>();

            CreateMap<RoleResponse, ApplicationRole>();

            CreateMap<UpdateRoleRequest, ApplicationRole>();

            CreateMap<ApplicationRoleClaim, RoleClaimViewModel>().ReverseMap();
        }
    }
}