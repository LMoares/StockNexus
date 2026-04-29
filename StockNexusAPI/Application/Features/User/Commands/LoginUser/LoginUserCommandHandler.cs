using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.DTOs.Users;
using StockNexusAPI.Application.Interfaces;
using StockNexusAPI.Infrastructure.Persistence;
using StockNexusAPI.Infrastructure.Services.Authentication;
namespace StockNexusAPI.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public LoginUserCommandHandler(ApplicationDbContext context, IJwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLower(), token);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };
            
            var jwtToken = _tokenGenerator.GenerateToken(userDto);

            return jwtToken;
        }
    }
}
