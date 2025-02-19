using AutoMapper;

using MediatR;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;

public sealed record GetUsersByIdRequest(int id) : IRequest<GetUsersByIdResponse>;