using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Queries
{
    public class GetAllUsersQuery : IRequest<IResponseWrapper>
    {

    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IResponseWrapper>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper; 
        }

        public async Task<IResponseWrapper> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var entities = await _userService.GetAllUsersAsync();

            if (entities.Any())
            {
                var mappedUsers = _mapper.Map<List<UserResponse>>(entities);
                return await ResponseWrapper<List<UserResponse>>.SuccessAsync(mappedUsers);
            }

            return await ResponseWrapper.FailAsync("No Users were found.");
        }
    }
}