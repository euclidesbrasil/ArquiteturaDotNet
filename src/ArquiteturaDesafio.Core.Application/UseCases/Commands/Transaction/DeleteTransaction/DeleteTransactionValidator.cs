using ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;
using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.DeleteTransaction;

public sealed class DeleteTransactionValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteTransactionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
