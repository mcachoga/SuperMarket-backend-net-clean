using Microsoft.AspNetCore.Mvc;
using SuperMarket.Application.Features.Products.Commands;
using SuperMarket.Application.Features.Products.Queries;
using SuperMarket.Common.Authorization;
using SuperMarket.Common.Requests.Products;
using SuperMarket.WebApi.Security;

namespace SuperMarket.WebApi.Controllers.Catalog
{
    [Route("api/[controller]")]
    public class ProductController : BaseGenericController<ProductController>
    {
        [HttpPost]
        [MustHavePermission(AppFeature.Products, AppAction.Create)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest createProduct)
        {
            var response = await MediatorSender.Send(new CreateProductCommand { CreateRequest = createProduct });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        [MustHavePermission(AppFeature.Products, AppAction.Update)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest updateProduct)
        {
            var response = await MediatorSender.Send(new UpdateProductCommand { UpdateRequest = updateProduct });
           
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("{productId}")]
        [MustHavePermission(AppFeature.Products, AppAction.Delete)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var response = await MediatorSender.Send(new DeleteProductCommand { ProductId = productId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet]
        [MustHavePermission(AppFeature.Products, AppAction.Read)]
        public async Task<IActionResult> GetProductList()
        {
            var response = await MediatorSender.Send(new GetProductsQuery());
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("{productId}")]
        [MustHavePermission(AppFeature.Products, AppAction.Read)]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var response = await MediatorSender.Send(new GetProductByIdQuery { ProductId = productId });
            
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}