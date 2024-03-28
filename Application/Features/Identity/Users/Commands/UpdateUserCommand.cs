using MediatR;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Identity.Users.Commands
{
    public class UpdateUserCommand : IRequest<IResponseWrapper>
    {
        public UpdateUserRequest UpdateUser { get; set; }
    }

    public class UpdateUserCommandHanlder : IRequestHandler<UpdateUserCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHanlder(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateUserAsync(request.UpdateUser);
        }
    }
}