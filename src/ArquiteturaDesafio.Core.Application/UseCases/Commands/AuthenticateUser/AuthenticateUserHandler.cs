using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser
{
    public class AuthenticateUserHandler  :
       IRequestHandler<AuthenticateUserRequest, AuthenticateUserResult>
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenService _tokenService;
        public AuthenticateUserHandler(IUserRepository repository, IMapper mapper,
            IConfiguration configuration, IJwtTokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }
        public async Task<AuthenticateUserResult> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByLoginAsync(request.Username);

            if (user == null  || !user.ValidatePassword(request.Password, user.PasswordHash, _tokenService) )
            {
                return new AuthenticateUserResult { Success = false, Message = "Invalid credentials" };
            }

            var token = _tokenService.GenerateJwtToken(user.Username,user.Role.ToString());
            return new AuthenticateUserResult { Success = true, Token = token, Message = "Authenticated" };
        }
    }
}
