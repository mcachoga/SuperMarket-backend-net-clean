using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Auth.Queries
{
    public class GetRefreshTokenQuery : IRequest<IResponseWrapper>
    {
        public RefreshTokenRequest RefreshTokenRequest { get; set; }
    }

    public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, IResponseWrapper>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public GetRefreshTokenQueryHandler(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var tokenAuth = await _tokenService.GetRefreshTokenAsync(request.RefreshTokenRequest.Token, request.RefreshTokenRequest.RefreshToken);

            var response = _mapper.Map<TokenResponse>(tokenAuth);
            
            return await ResponseWrapper<TokenResponse>.SuccessAsync(response);
        }
    }
}