using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockNexusAPI.Application.Features.Request.Commands.CreateRequest;
using StockNexusAPI.Application.Features.Request.Commands.ReviewRequest;
using System.Security.Claims;

namespace StockNexusAPI.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create([FromBody] CreateRequestCommand command)
        {
            // Set the EmployeeId from the authenticated user's claims
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id });
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Review([FromBody] ReviewRequestCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetRequestByIdQuery { Id = id });

            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
