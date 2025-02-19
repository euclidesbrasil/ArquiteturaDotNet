using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery;

namespace ArquiteturaDesafio.General.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest request,
                                                             CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        // PUT /users/{id}
        [HttpPut]
        public async Task<ActionResult<UpdateUserResponse>> Update(int id, [FromBody] UpdateUserRequest request,
                                                CancellationToken cancellationToken)
        {
            request.UpdateId(id);
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

      

        [HttpGet("/users/{id}")]
        public async Task<ActionResult<List<string>>> GetById(int id,CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetUsersByIdRequest(id), cancellationToken);
            return Ok(response);
        }

        [HttpGet("/users")]
        public async Task<ActionResult<List<GetUsersQueryResponse>>> GetUsersQuery(CancellationToken cancellationToken, int _page = 1, int _size = 10, [FromQuery] Dictionary<string, string> filters = null, string _order = "id asc")
        {
            filters = filters ?? new Dictionary<string, string>();
             filters = HttpContext.Request.Query
            .Where(q => q.Key != "_page" && q.Key != "_size" && q.Key != "_order")
            .ToDictionary(q => q.Key, q => q.Value.ToString());

            var response = await _mediator.Send(new GetUsersQueryRequest(_page, _size, _order, filters), cancellationToken);
            return Ok(response);
        }
    }
}
