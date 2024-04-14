using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;

namespace SuperMarket.Application.Features.Identity.Roles.Commands
{
    public class DeleteRoleCommand : IRequest<IResponseWrapper>
    {
        public string RoleId { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService;

        public DeleteRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IResponseWrapper> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleService.DeleteRoleAsync(request.RoleId);

            if (result.Succeeded)
            {
                return await ResponseWrapper.SuccessAsync("Role successfully deleted.");
            }

            return await ResponseWrapper.FailAsync(result.GetErrorDescriptions());
        }
    }
}