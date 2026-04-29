using StockNexusAPI.Application.DTOs.Users;

namespace StockNexusAPI.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserDto user);
    }
}
