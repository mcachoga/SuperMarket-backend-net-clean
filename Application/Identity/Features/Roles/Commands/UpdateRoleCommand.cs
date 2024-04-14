using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Roles.Commands
{
    public class UpdateRoleCommand : IRequest<IResponseWrapper>
    {
        public UpdateRoleRequest UpdateRole { get; set; }
    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ApplicationRole>(request.UpdateRole);

            var result = await _roleService.UpdateRoleAsync(request.UpdateRole.RoleId, entity);

            if (result.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("Role updated successfully.");
            }

            return await ResponseWrapper.FailAsync(result.GetErrorDescriptions());
        }
    }
}