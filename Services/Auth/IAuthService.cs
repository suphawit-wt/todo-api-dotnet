using todo_api_sqlserver.Models;
using todo_api_sqlserver.Models.Requests;

namespace todo_api_sqlserver.Services.Auth
{
    public interface IAuthService
    {
        Task<String> Login(LoginRequest req);
        Task Register(User req);
        int GetUserId();
    }
}
