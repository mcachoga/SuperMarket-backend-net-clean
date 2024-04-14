using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Auth;
using SuperMarket.Domain.Identity;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Persistence.Identity.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SuperMarket.Persistence.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtConfiguration _jwtConfiguration;

        public TokenService(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            IOptions<JwtConfiguration> jwtConfiguration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public async Task<TokenAuth> GetTokenAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new CustomAuthException("Invalid Credentials (user based -> info should be send to log).");
            }

            if (!user.IsActive)
            {
                throw new CustomAuthException("User not active. Please contact the administrator");
            }
            
            if (!user.EmailConfirmed)
            {
                throw new CustomAuthException("Email not confirmed.");
            }
            
            var isPaswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPaswordValid)
            {
                throw new CustomAuthException("Invalid Credentials. (password based -> info should be send to log).");
            }
            
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(_jwtConfiguration.TokenRefreshExpiryInDays);

            await _userManager.UpdateAsync(user);

            var token = await GenerateJWTAsync(user);

            var tokenAuth = new TokenAuth
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryDate
            };

            return tokenAuth;
        }

        public async Task<TokenAuth> GetRefreshTokenAsync(string token, string refreshToken)
        {
            if (token is null) throw new CustomAuthException("Invalid Client Token.");

            var userPrincipal = GetPrincipalFromExpiredToken(token);
            
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) throw new CustomAuthException("User Not Found.");

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) throw new CustomAuthException("User Not Found.");
            
            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryDate <= DateTime.Now)
                throw new CustomAuthException("Invalid Client Token.");
            
            var newToken = GenerateEncrytedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            user.RefreshToken = GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            var tokenAuth = new TokenAuth 
            { 
                Token = newToken, 
                RefreshToken = user.RefreshToken, 
                RefreshTokenExpiryTime = user.RefreshTokenExpiryDate 
            };

            return tokenAuth;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateJWTAsync(ApplicationUser user)
        {
            var token = GenerateEncrytedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            
            return token;
        }

        private string GenerateEncrytedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfiguration.TokenExpiryInMinutes), 
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
            
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var currentRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForCurrentRole = await _roleManager.GetClaimsAsync(currentRole);
                permissionClaims.AddRange(allPermissionsForCurrentRole);
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            return claims;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;
        }
    }
}