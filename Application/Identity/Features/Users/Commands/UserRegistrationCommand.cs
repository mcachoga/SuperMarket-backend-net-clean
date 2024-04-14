using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Commands
{
    public class UserRegistrationCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UserRegistrationRequest UserRegistration { get; set; }
    }

    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserRegistrationCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ApplicationUser>(request.UserRegistration);

            var result = await _userService.RegisterUserAsync(entity, request.UserRegistration.Password);

            if (result.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("User registered successfully.");
            }

            return await ResponseWrapper.FailAsync(result.GetErrorDescriptions());
        }
    }
}