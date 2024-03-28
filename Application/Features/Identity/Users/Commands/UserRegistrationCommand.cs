using MediatR;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Application.Validations.Pipelines;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Identity.Users.Commands
{
    public class UserRegistrationCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UserRegistrationRequest UserRegistration { get; set; }
    }

    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;

        public UserRegistrationCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IResponseWrapper> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RegisterUserAsync(request.UserRegistration);
        }
    }
}