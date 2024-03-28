using MediatR;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Identity.Token.Queries
{
    public class GetTokenQuery : IRequest<IResponseWrapper>
    {
        public TokenRequest TokenRequest { get; set; }
    }

    public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, IResponseWrapper>
    {
        private readonly ITokenService _tokenService;

        public GetTokenQueryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<IResponseWrapper> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            return await _tokenService.GetTokenAsync(request.TokenRequest);
        }
    }
}