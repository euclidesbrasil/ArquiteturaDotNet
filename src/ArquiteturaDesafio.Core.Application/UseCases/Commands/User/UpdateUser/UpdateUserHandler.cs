using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using ArquiteturaDesafio.Core.Domain.ValueObjects;

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
        var user = await _userRepository.Get(request.Id, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException($"Usuario não encontrado. Id: {request.Id}");
        }

        var adress = _mapper.Map<Address>(request.Address);
        user.UpdateUserInfo(request.Firstname, request.Lastname, adress, request.Phone, request.Status, request.Role);
        user.UpdateAdress(adress);
        user.UpdateEmail(request.Email);
        user.UpdateName(request.Firstname, request.Lastname);
        user.ChangePassword(request.Password, _tokenService);
        _userRepository.Update(user);
        await _unitOfWork.Commit(cancellationToken);
        return _mapper.Map<UpdateUserResponse>(user);
    }
}
