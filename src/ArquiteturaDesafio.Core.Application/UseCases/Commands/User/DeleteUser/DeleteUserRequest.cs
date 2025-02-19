using AutoMapper;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;

public class DeleteUserRequest : UserDTO, IRequest<DeleteUserResponse>
{
    public DeleteUserRequest(UserDTO user)
    {
        Id = user.Id;
        Firstname = user.Firstname;
        Lastname = user.Lastname;
        Email = user.Email;
    }
}
