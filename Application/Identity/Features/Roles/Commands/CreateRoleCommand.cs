using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Roles.Commands
{
    public class CreateRoleCommand : IRequest<IResponseWrapper>
    {
        public CreateRoleRequest CreateRole { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public CreateRoleCommandHandler(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ApplicationRole>(request.CreateRole);

            var result = await _roleService.CreateRoleAsync(entity);

            if (result.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("Role created successfully.");
            }

            return await ResponseWrapper<string>.FailAsync(result.GetErrorDescriptions());
        }
    }
}