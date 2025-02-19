using ArquiteturaDesafio.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(string username, string role);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
