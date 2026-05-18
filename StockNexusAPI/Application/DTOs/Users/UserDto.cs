using StockNexusAPI.Domain.Enums;
// This DTO is used to transfer user data between the application layers and generation of JWT tokens.
// The UserRole enumeration defines user roles such as: Admin, Manager, and Employee
// Roles are used to control access to certain features and endpoints in the application
namespace StockNexusAPI.Application.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
