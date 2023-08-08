using Xunit;
using Moq;
using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.Models.Request;
using TodoApp.Core.Models.Response;
using TodoApp.Core.Services;
using TodoApp.Core.ValueObjects;
using Bcrypt = BCrypt.Net.BCrypt;

namespace TodoApp.Tests.Unit.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _authService = new AuthService(_mockUserRepository.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task Login_Success()
        {
            var req = new LoginRequest
            {
                Username = "user01",
                Password = "123456az",
            };

            var mock = new User
            {
                Id = 1,
                Username = req.Username,
                Password = Bcrypt.HashPassword(req.Password),
                Email = "user01@mail.com",
                FirstName = "first01",
                LastName = "last01"
            };

            _mockUserRepository.Setup(repo => repo.GetByUsername(mock.Username)).ReturnsAsync(mock);
            _mockTokenService.Setup(service => service.GenerateAccessToken(mock.Id, UserRole.User)).Returns("mockJwtToken");

            TokenResponse actual = await _authService.Login(req);

            Assert.NotNull(actual);
            Assert.Equal("mockJwtToken", actual.AccessToken);
        }

        [Fact]
        public async Task Login_Invalid_Username()
        {
            var req = new LoginRequest
            {
                Username = "nonExistUsername",
                Password = "123456az",
            };

            var mock = new User
            {
                Id = 1,
                Username = "user01",
                Password = Bcrypt.HashPassword(req.Password),
                Email = "user01@mail.com",
                FirstName = "first01",
                LastName = "last01"
            };

            _mockUserRepository.Setup(repo => repo.GetByUsername(mock.Username)).ReturnsAsync(mock);

            await Assert.ThrowsAsync<UnauthorizedError>(() => _authService.Login(req));
        }

        [Fact]
        public async Task Login_Invalid_Password()
        {
            var req = new LoginRequest
            {
                Username = "user01",
                Password = "invalidPassword",
            };

            var mock = new User
            {
                Id = 1,
                Username = req.Username,
                Password = Bcrypt.HashPassword("123456az"),
                Email = "user01@mail.com",
                FirstName = "first01",
                LastName = "last01"
            };

            _mockUserRepository.Setup(repo => repo.GetByUsername(mock.Username)).ReturnsAsync(mock);

            await Assert.ThrowsAsync<UnauthorizedError>(() => _authService.Login(req));
        }

        [Fact]
        public async Task Register_Success()
        {
            var req = new RegisterRequest
            {
                Username = "user01",
                Password = "123456az",
                Email = "user01@mail.com",
                FirstName = "first01",
                LastName = "last01"
            };

            var mock = new User
            {
                Id = 1,
                Username = req.Username,
                Password = Bcrypt.HashPassword(req.Password),
                Email = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName
            };

            _mockUserRepository.Setup(repo => repo.Create(mock)).ReturnsAsync(mock);

            await _authService.Register(req);
        }

        [Fact]
        public async Task Register_Username_Taken()
        {
            var req = new RegisterRequest
            {
                Username = "user01",
                Password = "123456az",
                Email = "thisIsNewEmail@mail.com",
                FirstName = "first01",
                LastName = "last01"
            };

            var mock = new User
            {
                Id = 1,
                Username = "user01",
                Password = Bcrypt.HashPassword(req.Password),
                Email = "user01@mail.com",
                FirstName = req.FirstName,
                LastName = req.LastName
            };

            _mockUserRepository.Setup(repo => repo.Create(It.Is<User>(
                u => u.Username == mock.Username
            ))).ThrowsAsync(new ConflictError("This Username already taken"));

            await Assert.ThrowsAsync<ConflictError>(() => _authService.Register(req));
        }

        [Fact]
        public async Task Register_Email_Taken()
        {
            var req = new RegisterRequest
            {
                Username = "thisIsNewUsername",
                Password = "123456az",
                Email = "user01@mail.com",
                FirstName = "first01",
                LastName = "last01"
            };

            var mock = new User
            {
                Id = 1,
                Username = "user01",
                Password = Bcrypt.HashPassword(req.Password),
                Email = "user01@mail.com",
                FirstName = req.FirstName,
                LastName = req.LastName
            };

            _mockUserRepository.Setup(repo => repo.Create(It.Is<User>(
                u => u.Email == mock.Email
            ))).ThrowsAsync(new ConflictError("This Email already taken"));

            await Assert.ThrowsAsync<ConflictError>(() => _authService.Register(req));
        }

    }
}