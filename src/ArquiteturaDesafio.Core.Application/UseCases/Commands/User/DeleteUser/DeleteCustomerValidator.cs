using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;

public sealed class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
