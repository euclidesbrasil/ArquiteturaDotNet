using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation;
namespace ArquiteturaDesafio.General.Api.Filters;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        switch (exception)
        {
            // Erros de validação (400 Bad Request)
            case ValidationException validationException:
                var errors = validationException.Errors
                    .Select(error => $"{error.PropertyName}: {error.ErrorMessage}")
                    .ToList();

                context.Result = new BadRequestObjectResult(new
                {
                    Title = "Validation erros.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = errors
                });
                context.ExceptionHandled = true;
                break;

            // Recurso não encontrado (404 Not Found)
            case KeyNotFoundException _:
            case ArgumentNullException _:
                context.Result = new NotFoundObjectResult(new
                {
                    Title = "Resource not found.",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = exception.Message
                });
                context.ExceptionHandled = true;
                break;

            // Erros de solicitação inválida (400 Bad Request)
            case ArgumentException _:
            case InvalidOperationException _:
                context.Result = new BadRequestObjectResult(new
                {
                    Title = "Invalid request.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = exception.Message
                });
                context.ExceptionHandled = true;
                break;

            // Erros de autenticação/autorização (401 Unauthorized ou 403 Forbidden)
            case UnauthorizedAccessException _:
                context.Result = new ObjectResult(new
                {
                    Title = "Unauthorized.",
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = exception.Message
                })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                context.ExceptionHandled = true;
                break;

            // Erros internos do servidor (500 Internal Server Error)
            default:
                context.Result = new ObjectResult(new
                {
                    Title = "Internal server error.",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde."
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                context.ExceptionHandled = true;
                break;
        }

        // Log da exceção (opcional, dependendo do seu sistema de logs)
        LogException(exception);
    }

    private void LogException(Exception exception)
    {
        // Implemente o registro de logs aqui (ex.: usando ILogger, Serilog, etc.)
        Console.WriteLine($"Erro: {exception.Message}");
        Console.WriteLine($"StackTrace: {exception.StackTrace}");
    }
}