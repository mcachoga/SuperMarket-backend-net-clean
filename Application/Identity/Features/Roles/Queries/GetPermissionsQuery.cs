using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Roles.Queries
{
    public class GetPermissionsQuery : IRequest<IResponseWrapper>
    {
        public string RoleId { get; set; }
    }

    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IResponseWrapper>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public GetPermissionsQueryHandler(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetRoleByIdAsync(request.RoleId);
            
            if (role is null)
            {
                return await ResponseWrapper.FailAsync("Role does not exist.");
            }

            var result = await _roleService.GetAllClaimsForRoleAsync(request.RoleId);
            var currentRoleClaims = _mapper.Map<List<RoleClaimViewModel>>(result);

            var allPermissions = AppPermissions.AllPermissions;
            var roleClaimResponse = new RoleClaimResponse
            {
                Role = new()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                },
                RoleClaims = new()
            };

            var allPermissionsNames = allPermissions.Select(p => p.Name).ToList();
            var currentRoleClaimsValues = currentRoleClaims.Select(crc => crc.ClaimValue).ToList();

            var currentlyAssignedRoleClaimsNames = allPermissionsNames.Intersect(currentRoleClaimsValues).ToList();

            foreach (var permission in allPermissions)
            {
                var roleClaimViewModel = new RoleClaimViewModel()
                {
                    RoleId = role.Id,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.Name,
                    Description = permission.Description,
                    Group = permission.Group,
                    IsAssignedToRole = currentlyAssignedRoleClaimsNames.Any(carc => carc == permission.Name)
                };

                roleClaimResponse.RoleClaims.Add(roleClaimViewModel);
            }

            return await ResponseWrapper<RoleClaimResponse>.SuccessAsync(roleClaimResponse);
        }
    }
}