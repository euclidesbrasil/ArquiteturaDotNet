using AutoMapper;

using MediatR;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;

public sealed record GetUsersByIdRequest(Guid id) : IRequest<GetUsersByIdResponse>;