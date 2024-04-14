using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Application.Features.Auth.Queries;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.WebApi.Controllers.Auth
{
    [Route("api/[controller]")]
    public class SystemController : BaseGenericController<SystemController>
    {
        //[HttpGet("system/version")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetSystemVersionAsync()
        //{
        //    var response = await MediatorSender.Send(new GetTokenQuery { TokenRequest = tokenRequest });
            
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest(response);
        //}

        //[HttpGet("system/stats")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetStatsAsync()
        //{
        //    var response = await MediatorSender.Send(new GetRefreshTokenQuery { RefreshTokenRequest = refreshTokenRequest });
            
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }
            
        //    return BadRequest(response);
        //}
    }
}