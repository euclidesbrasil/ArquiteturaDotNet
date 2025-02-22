using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;
public sealed class GetUsersByIdHandler : IRequestHandler<GetUsersByIdRequest, GetUsersByIdResponse>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetUsersByIdHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetUsersByIdResponse> Handle(GetUsersByIdRequest query, CancellationToken cancellationToken)
    {

        User user = await _repository.Get(query.id, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException($"Usuario não encontrado. Id: {query.id}");
        }

        return _mapper.Map<GetUsersByIdResponse>(user);
    }
}
