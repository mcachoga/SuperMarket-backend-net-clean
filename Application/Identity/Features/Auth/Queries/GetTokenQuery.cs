using AutoMapper;
using MediatR;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Requests.Identity;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Auth.Queries
{
    public class GetTokenQuery : IRequest<IResponseWrapper>
    {
        public TokenRequest TokenRequest { get; set; }
    }

    public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, IResponseWrapper>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public GetTokenQueryHandler(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var tokenAuth = await _tokenService.GetTokenAsync(request.TokenRequest.Email, request.TokenRequest.Password);

            var response = _mapper.Map<TokenResponse>(tokenAuth);

            return await ResponseWrapper<TokenResponse>.SuccessAsync(response);
        }
    }
}