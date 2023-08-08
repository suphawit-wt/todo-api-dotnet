using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;
using TodoApp.Core.ValueObjects;
using Bcrypt = BCrypt.Net.BCrypt;

namespace TodoApp.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> Login(LoginRequest req)
        {
            User? user = await _userRepository.GetByUsername(req.Username) ?? throw new UnauthorizedError("Username or Password is invalid.");

            bool passwordVerified = Bcrypt.Verify(req.Password, user.Password);
            if (!passwordVerified)
            {
                throw new UnauthorizedError("Username or Password is invalid.");
            }

            string accessToken = _tokenService.GenerateAccessToken(user.Id, UserRole.User);

            return new TokenResponse(accessToken);
        }

        public async Task Register(RegisterRequest req)
        {
            var user = new User
            {
                Username = req.Username,
                Password = Bcrypt.HashPassword(req.Password),
                Email = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName
            };

            await _userRepository.Create(user);
        }

    }
}