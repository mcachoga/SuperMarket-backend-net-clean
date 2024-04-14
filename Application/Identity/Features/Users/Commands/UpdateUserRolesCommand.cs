using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Commands
{
    public class UpdateUserRolesCommand : IRequest<IResponseWrapper>
    {
        public UpdateUserRolesRequest UpdateUserRoles { get; set; }
    }

    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;

        public UpdateUserRolesCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IResponseWrapper> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var rolesToBeAssigned = request.UpdateUserRoles.Roles.Where(role => role.IsAssignedToUser == true).ToList();
            var roleNamesToAssign = rolesToBeAssigned.Select(role => role.RoleName).ToList();

            var result = await _userService.UpdateUserRolesAsync(request.UpdateUserRoles.UserId, roleNamesToAssign);

            if (result.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("User Roles Updated Successfully.");
            }

            return await ResponseWrapper.FailAsync(result.GetErrorDescriptions());
        }
    }
}