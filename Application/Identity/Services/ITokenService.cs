using SuperMarket.Domain.Auth;

namespace SuperMarket.Application.Identity.Services.Contracts
{
    public interface ITokenService
    {
        Task<TokenAuth> GetTokenAsync(string email, string password);

        Task<TokenAuth> GetRefreshTokenAsync(string token, string refreshToken);
    }
}