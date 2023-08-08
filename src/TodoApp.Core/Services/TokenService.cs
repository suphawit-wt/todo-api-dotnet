using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Core.Constants;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces.Services;
using TodoApp.Core.ValueObjects;
using JwtClaims = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace TodoApp.Core.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateAccessToken(int userId, UserRole role)
        {
            var claims = new List<Claim> {
                new Claim(JwtClaims.Sub, userId.ToString()),
                new Claim("role", role.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.JWT_SECRET));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Const.JWT_ISSUER,
                audience: Const.JWT_AUDIENCE,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserId(ClaimsPrincipal user)
        {
            string userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedError("Invalid Credentials");

            return int.Parse(userId);
        }

    }
}