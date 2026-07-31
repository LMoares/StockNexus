using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StockNexusAPI.Application.Features.User.Commands.RegisterAdmin;
using StockNexusAPI.Application.Features.User.Commands.RegisterUser;

namespace StockNexusAPI.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [EnableRateLimiting("StandardApiPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admins")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterAdminCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(201, new { Message = "Admin user created successfully." });
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(201, new { Message = "User registered successfully." });
        }
    }
}
