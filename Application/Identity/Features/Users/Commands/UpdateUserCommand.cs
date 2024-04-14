using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Extensions;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Commands
{
    public class UpdateUserCommand : IRequest<IResponseWrapper>
    {
        public UpdateUserRequest UpdateUser { get; set; }
    }

    public class UpdateUserCommandHanlder : IRequestHandler<UpdateUserCommand, IResponseWrapper>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UpdateUserCommandHanlder(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<ApplicationUser>(request.UpdateUser);

            var result = await _userService.UpdateUserAsync(request.UpdateUser.UserId, entity);

            if (result.Succeeded)
            {
                return await ResponseWrapper<string>.SuccessAsync("User details successfully updated.");
            }

            return await ResponseWrapper.FailAsync(result.GetErrorDescriptions());
        }
    }
}