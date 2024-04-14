using Microsoft.AspNetCore.Mvc;
using SuperMarket.Application.Features.Orders.Commands;
using SuperMarket.Application.Features.Orders.Queries;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Shared.Requests.Catalog;

namespace SuperMarket.WebApi.Controllers.Catalog
{
    [Route("api/[controller]")]
    public class OrderController : BaseGenericController<OrderController>
    {
        [HttpPost]
        [MustHavePermission(AppFeature.Orders, AppAction.Create)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrder)
        {
            var response = await MediatorSender.Send(new CreateOrderCommand { CreateRequest = createOrder });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        [MustHavePermission(AppFeature.Orders, AppAction.Update)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest updateOrder)
        {
            var response = await MediatorSender.Send(new UpdateOrderCommand { UpdateRequest = updateOrder });
           
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("{productId}")]
        [MustHavePermission(AppFeature.Orders, AppAction.Delete)]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var response = await MediatorSender.Send(new DeleteOrderCommand { OrderId = orderId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("all")]
        [MustHavePermission(AppFeature.Orders, AppAction.Read)]
        public async Task<IActionResult> GetOrderList()
        {
            var response = await MediatorSender.Send(new GetOrdersQuery());
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("{productId}")]
        [MustHavePermission(AppFeature.Orders, AppAction.Read)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var response = await MediatorSender.Send(new GetOrderByIdQuery { OrderId = orderId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}