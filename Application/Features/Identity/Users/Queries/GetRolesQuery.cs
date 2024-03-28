using MediatR;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Identity.Users.Queries
{
    public class GetRolesQuery : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IResponseWrapper>
    {
        private readonly IUserService _userService;

        public GetRolesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IResponseWrapper> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetRolesAsync(request.UserId);
        }
    }
}