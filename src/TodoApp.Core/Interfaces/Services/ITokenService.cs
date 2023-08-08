using System.Security.Claims;
using TodoApp.Core.ValueObjects;

namespace TodoApp.Core.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(int userId, UserRole role);
        int GetUserId(ClaimsPrincipal user);
    }
}