using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Infrastructure.Persistence;
namespace StockNexusAPI.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public RegisterUserCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken token)
        {
            Domain.Entities.User? managerExists = null;
            
            if (request.ManagerId != null)
            {
                managerExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.ManagerId.Value, token);
            }

            var user = new Domain.Entities.User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                ManagerId = request.ManagerId,
                Manager = managerExists
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(token);

            return Unit.Value;
        }
    }
}
