using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockNexusAPI.Application.Features.Product.Commands.DeleteProduct;
using StockNexusAPI.Application.Features.Product.Commands.RegisterProduct;
using StockNexusAPI.Application.Features.Product.Queries.GetProductById;
using StockNexusAPI.Application.Features.Product.Queries.GetProducts;

namespace StockNexusAPI.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> RegisterProduct([FromBody] RegisterProductCommand command)
        {
            var result = await _mediator.Send(command);

            return StatusCode(201, new { Message = "Product registered successfully"});
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });

            return Ok(result);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            return Ok(new { Message = "Product deleted successfully" });
        }
    }
}
