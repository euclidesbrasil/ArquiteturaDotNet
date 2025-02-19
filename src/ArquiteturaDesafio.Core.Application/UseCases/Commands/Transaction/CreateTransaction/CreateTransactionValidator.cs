using ArquiteturaDesafio.Core.Domain.Enum;
using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;

public sealed class CreateTransactionValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Type).Must(type => type == TransactionType.Credit || type == TransactionType.Debit)
                             .WithMessage("Transação deve ser Credit ou Debit");
        RuleFor(x => x.Amount).NotNull();
        RuleFor(x => x.Amount.Amount).GreaterThan(0);
    }
}
