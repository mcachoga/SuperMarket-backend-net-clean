using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Roles.Queries
{
    public class GetRolesQuery : IRequest<IResponseWrapper>
    {

    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IResponseWrapper>
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _roleService.GetRolesAsync();

            if (entities.Any())
            {
                var mappedRoles = _mapper.Map<List<RoleResponse>>(entities);
                return await ResponseWrapper<List<RoleResponse>>.SuccessAsync(mappedRoles);
            }

            return await ResponseWrapper<string>.FailAsync("No roles were found.");
        }
    }
}