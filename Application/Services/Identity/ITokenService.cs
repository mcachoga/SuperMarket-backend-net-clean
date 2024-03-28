using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Services.Identity
{
    public interface ITokenService
    {
        Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest tokenRequest);

        Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}