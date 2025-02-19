using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;

public class DeleteUserResponse : UserDTO
{
    public DeleteUserResponse(UserDTO user)
    {
        Id = user.Id;
        Firstname = user.Firstname;
        Lastname = user.Lastname;
        Email = user.Email;
    }
}