using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser
{
    public class UpdateUserRequest : UserBaseDTO, IRequest<UpdateUserResponse>
    {
        public Guid Id { get; internal set; }

        public void UpdateId(Guid id)
        {
            Id = id;
        }
    }
}
