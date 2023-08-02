using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using todo_api_sqlserver.Data;
using todo_api_sqlserver.Models;
using todo_api_sqlserver.Models.Requests;

namespace todo_api_sqlserver.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dbcontext;
        private IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext dbcontext, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbcontext;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Login(LoginRequest req)
        {
            var user = await _dbcontext.Users.Where(x => x.Username == req.Username).FirstOrDefaultAsync() ?? throw new Exception("401");

            var passwordVerify = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);

            if (passwordVerify == false)
            {
                throw new Exception("401");
            }

            var accessToken = GenerateAccessToken(user);

            return accessToken;
        }

        public async Task Register(User req)
        {
            var usernameCheck = await _dbcontext.Users.Where(x => x.Username == req.Username).FirstOrDefaultAsync();

            if (usernameCheck != null)
            {
                throw new Exception("409");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);

            var user = new User
            {
                Username = req.Username,
                Password = passwordHash,
                Email = req.Email,
                First_Name = req.First_Name,
                Last_Name = req.Last_Name
            };

            await _dbcontext.Users.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
        }

        public int GetUserId()
        {
            var claims = _httpContextAccessor?.HttpContext?.User.Identity as ClaimsIdentity;
            var claimUserId = claims?.FindFirst("user_id")?.Value ?? throw new Exception("401");
            int userId = int.Parse(claimUserId);

            return userId;
        }

        private string GenerateAccessToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Secret").Value!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config.GetSection("JWT:Issuer").Value,
                audience: _config.GetSection("JWT:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
