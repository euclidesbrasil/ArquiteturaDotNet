using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.DeleteTransaction;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsById;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsQuery;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;

namespace ArquiteturaDesafio.General.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateTransactionResponse>> Create(CreateTransactionRequest request,
                                                             CancellationToken cancellationToken)
        {
            // Definido a data parametro como o agora como padrão;
            request.UpdateDate(DateTime.UtcNow.Date);

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateTransactionResponse>> Update(Guid id, [FromBody] UpdateTransactionRequest request,
                                                CancellationToken cancellationToken)
        {
            request.UpdateId(id);
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id,
                                                CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DeleteTransactionRequest(id), cancellationToken);
            return Ok(response);
        }

        [HttpGet("/Transaction/{id}")]
        public async Task<ActionResult<GetTransactionsByIdResponse>> GetById(Guid id,CancellationToken cancellationToken)
        {
            var request = new GetTransactionsByIdRequest(id);
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("/Transaction")]
        public async Task<ActionResult<GetTransactionsQueryResponse>> GetAllQuery(CancellationToken cancellationToken, int _page = 1, int _size = 10, [FromQuery] Dictionary<string, string> filters = null, string _order = "id asc")
        {
            filters = filters ?? new Dictionary<string, string>();
             filters = HttpContext.Request.Query
            .Where(q => q.Key != "_page" && q.Key != "_size" && q.Key != "_order")
            .ToDictionary(q => q.Key, q => q.Value.ToString());

            var response = await _mediator.Send(new GetTransactionsQueryRequest(_page, _size, _order, filters), cancellationToken);
            return Ok(response);
        }
    }
}
