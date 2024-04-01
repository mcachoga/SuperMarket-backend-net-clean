using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using SuperMarket.Application.Features.Employees.Queries;
using SuperMarket.Application.Features.Markets.Commands;
using SuperMarket.Common.Authorization;
using SuperMarket.Common.Requests.Markets;
using SuperMarket.WebApi.Security;

namespace SuperMarket.WebApi.Controllers.Catalog
{
    [Route("api/[controller]")]
    public class MarketController : BaseGenericController<MarketController>
    {
        [HttpPost]
        [MustHavePermission(AppFeature.Markets, AppAction.Create)]
        public async Task<IActionResult> CreateMarket([FromBody] CreateMarketRequest createMarket)
        {
            LogContext.PushProperty("Route", "api/market/CreateMarket");
            LogContext.PushProperty("User", "dfdfdfdfdf");
            base._logger.LogInformation("Mensaje de prueba");


            var response = await MediatorSender.Send(new CreateMarketCommand { CreateRequest = createMarket });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        [MustHavePermission(AppFeature.Markets, AppAction.Update)]
        public async Task<IActionResult> UpdateMarket([FromBody] UpdateMarketRequest updateMarket)
        {
            var response = await MediatorSender.Send(new UpdateMarketCommand { UpdateRequest = updateMarket });
           
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("{marketId}")]
        [MustHavePermission(AppFeature.Markets, AppAction.Delete)]
        public async Task<IActionResult> DeleteMarket(int marketId)
        {
            var response = await MediatorSender.Send(new DeleteMarketCommand { MarketId = marketId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet]
        [MustHavePermission(AppFeature.Markets, AppAction.Read)]
        public async Task<IActionResult> GetMarketList()
        {
            LogContext.PushProperty("Route", "api/market/GetMarketList");
            LogContext.PushProperty("UserId", "dfdfdfdfdf");
            _logger.LogInformation("Mensaje de prueba de marketList");

            var response = await MediatorSender.Send(new GetMarketsQuery());
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("{marketId}")]
        [MustHavePermission(AppFeature.Markets, AppAction.Read)]
        public async Task<IActionResult> GetMarketById(int marketId)
        {
            var response = await MediatorSender.Send(new GetMarketByIdQuery { MarketId = marketId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}