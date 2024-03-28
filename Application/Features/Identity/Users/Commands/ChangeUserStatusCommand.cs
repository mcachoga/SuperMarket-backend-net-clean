using MediatR;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Identity.Users.Commands
{
    public class ChangeUserStatusCommand : IRequest<IResponseWrapper>
    {
        public ChangeUserStatusRequest ChangeUserStatus { get; set; }
    }

    public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;

        public ChangeUserStatusCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IResponseWrapper> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            return await _userService.ChangeUserStatusAsync(request.ChangeUserStatus);
        }
    }
}