using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Commands
{
    public class ChangeUserPasswordCommand : IRequest<IResponseWrapper>
    {
        public ChangePasswordRequest ChangePassword { get; set; }
    }

    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;

        public ChangeUserPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IResponseWrapper> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.ChangeUserPasswordAsync(
                request.ChangePassword.UserId, 
                request.ChangePassword.CurrentPassword, 
                request.ChangePassword.NewPassword);

            if (result.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("User password updated.");
            }

            return await ResponseWrapper.FailAsync(result.GetErrorDescriptions());
        }
    }
}