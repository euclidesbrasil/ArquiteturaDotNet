using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser
{
    public class AuthenticateUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
