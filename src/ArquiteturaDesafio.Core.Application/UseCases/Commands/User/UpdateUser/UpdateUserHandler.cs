using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;

public class UpdateUserHandler :
       IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _tokenService;

    public UpdateUserHandler(IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IMapper mapper,
        IJwtTokenService tokenService
        )
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request,
        CancellationToken cancellationToken)
    {

        var user = _mapper.Map<ArquiteturaDesafio.Core.Domain.Entities.User>(request);
        user.UpdateName(request.Firstname, request.Lastname);
        user.ChangePassword(request.Password, _tokenService);
        _userRepository.Update(user);
        await _unitOfWork.Commit(cancellationToken);
        return _mapper.Map<UpdateUserResponse>(user);
    }
}
