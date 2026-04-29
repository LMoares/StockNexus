using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockNexusAPI.Application.Features.User.Commands.LoginUser;
using StockNexusAPI.Application.Features.User.Commands.RegisterUser;

namespace StockNexusAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new { Message = "Admin user created successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var jwtToken = await _mediator.Send(command);
            return Ok(new { Token = jwtToken });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "User registered successfully." });
        }
    }
 }