using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;

namespace TodoApp.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<TokenResponse> Login(LoginRequest req);
        Task Register(RegisterRequest req);
    }
}