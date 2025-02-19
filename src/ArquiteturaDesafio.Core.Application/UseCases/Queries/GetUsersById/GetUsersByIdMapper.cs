using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;

public sealed class GetUsersByIdMapper : Profile
{
    public GetUsersByIdMapper()
    {
        CreateMap<User, GetUsersByIdResponse>();
    }
}
