using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetDailyReportQuery;
using ArquiteturaDesafio.Core.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
[Route("[controller]")]
[ApiController]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/Report/Daily/Mongodb/")]
    public async Task<IActionResult> GetDailyReport([FromQuery] DateTime? date)
    {
        // Se não for passado, usa a data atual (UTC)
        var reportDate = (date ?? DateTime.UtcNow).Date;

        var query = new GetDailyReportQueryRequest(reportDate, DatabaseType.Mongo);
        
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // Endpoint para PostgreSQL
    [HttpGet("/Report/Daily/Postgres/")]
    public async Task<IActionResult> GetDailyReportPostgres([FromQuery] DateTime? date)
    {
        var reportDate = (date ?? DateTime.UtcNow).Date;
        var query = new GetDailyReportQueryRequest(reportDate, ArquiteturaDesafio.Core.Domain.Enum.DatabaseType.Postgree);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}