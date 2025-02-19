using ArquiteturaDesafio.Core.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ArquiteturaDesafio.Core.Domain.Interfaces;
namespace ArquiteturaDesafio.Infrastructure.Security
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(string username, string role)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string HashPassword(string password)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            using (var hmac = new System.Security.Cryptography.HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPassword(string password,string hashedPassword)
        {
            string key = _config["Jwt:Key"];
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}
