using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Queries
{
    public class GetUserByIdQuery : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }

    public class GetUserByIdQueryHanlder : IRequestHandler<GetUserByIdQuery, IResponseWrapper>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHanlder(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _userService.GetUserByIdAsync(request.UserId);

            var mappedUser = _mapper.Map<UserResponse>(entity);
            return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
        }
    }
}