using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser
{
    public class AuthenticateUserRequest : IRequest<AuthenticateUserResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
