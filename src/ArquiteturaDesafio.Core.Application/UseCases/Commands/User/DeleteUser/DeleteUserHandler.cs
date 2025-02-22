using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DeleteUserHandler(IUnitOfWork unitOfWork,
        IUserRepository productRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _userRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        ArquiteturaDesafio.Core.Domain.Entities.User user = await _userRepository.Get(request.Id, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"Usuario não encontrado. Id: {request.Id}");
        }

        _userRepository.Delete(user);
        await _unitOfWork.Commit(cancellationToken);
        return _mapper.Map<DeleteUserResponse>(user);
    }
}
