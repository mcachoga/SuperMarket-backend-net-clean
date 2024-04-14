using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Roles.Queries
{
    public class GetRoleByIdQuery : IRequest<IResponseWrapper>
    {
        public string RoleId { get; set; }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, IResponseWrapper>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _roleService.GetRoleByIdAsync(request.RoleId);

            var mappedRole = _mapper.Map<RoleResponse>(entity);
            return await ResponseWrapper<RoleResponse>.SuccessAsync(mappedRole);
        }
    }
}