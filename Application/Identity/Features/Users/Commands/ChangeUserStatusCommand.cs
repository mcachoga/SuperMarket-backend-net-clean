using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;

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
            var result = await _userService.ChangeUserStatusAsync(request.ChangeUserStatus.UserId, request.ChangeUserStatus.Activate);

            if (result.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync(request.ChangeUserStatus.Activate ? 
                    "User actived successfully." : "User de-activated successfully");
            }

            return await ResponseWrapper.FailAsync(result.GetErrorDescriptions());
        }
    }
}