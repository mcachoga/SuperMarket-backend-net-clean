using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Roles.Commands
{
    public class UpdateRolePermissionsCommand : IRequest<IResponseWrapper>
    {
        public UpdateRolePermissionsRequest UpdateRolePermissions { get; set; }
    }

    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UpdateRolePermissionsCommandHandler(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var permissionsAssignedToRole = request.UpdateRolePermissions.RoleClaims.Where(rc => rc.IsAssignedToRole == true).ToList();

            IList<ApplicationRoleClaim> permissionsToBeAssigned = new List<ApplicationRoleClaim>();
            foreach (var claim in permissionsAssignedToRole)
            {
                var entity = _mapper.Map<ApplicationRoleClaim>(claim);
                permissionsToBeAssigned.Add(entity);
            }

            await _roleService.UpdateRolePermissionsAsync(request.UpdateRolePermissions.RoleId, permissionsToBeAssigned);
            
            return await ResponseWrapper<string>.SuccessAsync("Role permissions updated successfully.");
        }
    }
}