using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Queries
{
    public class GetRolesQuery : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IResponseWrapper>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public GetRolesQueryHandler(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<IResponseWrapper> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var userRoles = await _userService.GetUserRolesAsync(request.UserId);
            var userRolesNames = userRoles.Select(x => x.Name).ToList();

            var userRolesVM = new List<UserRoleViewModel>();

            var allRoles = await _roleService.GetRolesAsync();

            foreach (var role in allRoles)
            {
                var userRoleVM = new UserRoleViewModel
                {
                    RoleName = role.Name,
                    RoleDescription = role.Description,
                    IsAssignedToUser = userRolesNames.Contains(role.Name)
                };

                userRolesVM.Add(userRoleVM);
            }

            return await ResponseWrapper<List<UserRoleViewModel>>.SuccessAsync(userRolesVM);
        }
    }
}