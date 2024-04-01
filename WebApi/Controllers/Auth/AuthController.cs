using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using SuperMarket.Application.Features.Identity.Token.Queries;
using SuperMarket.Common.Requests.Identity;

namespace SuperMarket.WebApi.Controllers.Auth
{
    [Route("api/[controller]")]
    public class AuthController : BaseGenericController<AuthController>
    {
        [HttpPost("get-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest tokenRequest)
        {
            LogContext.PushProperty("Route", "api/auth/get-token");
            LogContext.PushProperty("UserId", "");
            Log.Logger.Information("GetTokenAsync");

            var response = await MediatorSender.Send(new GetTokenQuery { TokenRequest = tokenRequest });
            
            if (response.IsSuccessful)
            {
                Log.Logger.Information("GetTokenAsync OK");
                return Ok(response);
            }

            Log.Logger.Information("GetTokenAsync KO");
            return BadRequest(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> GetRefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var response = await MediatorSender.Send(new GetRefreshTokenQuery { RefreshTokenRequest = refreshTokenRequest });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
    }
}