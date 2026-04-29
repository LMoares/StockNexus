using MediatR;
using StockNexusAPI.Domain.Enums;
namespace StockNexusAPI.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Unit>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? ManagerId { get; set; }

        public UserRole Role { get; set; }
    }
}
