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

        public AuthService(DataContext dbcontext, IConfiguration config)
        {
            _dbcontext = dbcontext;
            _config = config;
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

        public string GenerateAccessToken(User user)
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
