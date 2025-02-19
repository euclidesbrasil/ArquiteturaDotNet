using ArquiteturaDesafio.Core.Domain.Enum;
using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;

public sealed class UpdateTransactionValidator : AbstractValidator<UpdateTransactionRequest>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Type).Must(type => type == TransactionType.Credit || type == TransactionType.Debit)
                              .WithMessage("Transação deve ser Credit ou Debit");
        RuleFor(x => x.Amount.Amount).GreaterThan(0);
    }
}
