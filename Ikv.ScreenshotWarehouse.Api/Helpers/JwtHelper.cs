using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Ikv.ScreenshotWarehouse.Api.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Startup.JwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("id", user.Id.ToString()),
                        new Claim("email", user.Email),
                        new Claim("username", user.Username),
                        new Claim("role", string.IsNullOrEmpty(user.Role) ? "user" : user.Role),
                    }),
                Expires = DateTime.UtcNow.AddDays(3650),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}