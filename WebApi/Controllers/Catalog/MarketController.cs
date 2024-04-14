using Microsoft.AspNetCore.Mvc;
using SuperMarket.Application.Features.Markets.Commands;
using SuperMarket.Application.Features.Markets.Queries;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Shared.Requests.Catalog;

namespace SuperMarket.WebApi.Controllers.Catalog
{
    [Route("api/[controller]")]
    public class MarketController : BaseGenericController<MarketController>
    {
        [HttpPost]
        [MustHavePermission(AppFeature.Markets, AppAction.Create)]
        public async Task<IActionResult> CreateMarket([FromBody] CreateMarketRequest createMarket)
        {
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

        [HttpGet("all")]
        [MustHavePermission(AppFeature.Markets, AppAction.Read)]
        public async Task<IActionResult> GetMarketList()
        {
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