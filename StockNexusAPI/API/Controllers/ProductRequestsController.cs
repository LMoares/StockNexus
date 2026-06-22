using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StockNexusAPI.Application.Features.ProductRequest.Queries.GetEmployeeProductRequests;
using StockNexusAPI.Application.Features.ProductRequest.Queries.GetManagerProductRequests;
using StockNexusAPI.Application.Features.Request.Commands.CreateRequest;
using StockNexusAPI.Application.Features.Request.Commands.ReviewRequest;
using StockNexusAPI.Application.Features.Request.Queries.GetRequestById;
using StockNexusAPI.Domain.Enums;

namespace StockNexusAPI.API.Controllers
{
    [ApiController]
    [Route("api/productRequests")]
    [EnableRateLimiting("StandardApiPolicy")]
    public class ProductRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create([FromBody] CreateRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = result}, new { Id = result});
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Review([FromBody] ReviewRequestCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("employee")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetEmployeeRequests([FromQuery] RequestStatus? statusFilter)
        {
            var result = await _mediator.Send(new GetEmployeeProductRequestsQuery { StatusFilter = statusFilter });
            return Ok(result);
        }

        // 4. READ COLLECTION: Manager scoped list
        [HttpGet("manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetManagerRequests([FromQuery] RequestStatus? statusFilter)
        {
            var result = await _mediator.Send(new GetManagerProductRequestsQuery { StatusFilter = statusFilter });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetProductRequestByIdQuery { Id = id });

            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
