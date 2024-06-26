﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Application.Features.Auth.Queries;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.WebApi.Controllers.Auth
{
    [Route("api/[controller]")]
    public class AuthController : BaseGenericController<AuthController>
    {
        [HttpPost("get-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest tokenRequest)
        {
            Logger.LogInformation("GetTokenAsync -> Con _logger");

            var response = await MediatorSender.Send(new GetTokenQuery { TokenRequest = tokenRequest });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

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