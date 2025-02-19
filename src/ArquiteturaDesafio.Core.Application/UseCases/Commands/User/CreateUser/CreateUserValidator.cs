using FluentValidation;

namespace ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.Email).EmailAddress();
    }
}
