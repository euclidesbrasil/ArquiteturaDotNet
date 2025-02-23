using ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArquiteturaDesafio.General.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IConfiguration config, IJwtTokenService jwtTokenService, IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest command, CancellationToken cancellationToken)
        {
            AuthenticateUserResult response = await _mediator.Send(command, cancellationToken);

            return Ok(new { token=response.Token });

            // return Unauthorized();
        }
    }
}
