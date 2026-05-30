using FluentValidation;

namespace Board.Application.Features.Users.CreateUser;

internal sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
            .WithMessage("O e-mail informado não é válido.")
            .MaximumLength(255)
            .WithMessage("O e-mail deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.")
            .MinimumLength(8)
            .WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .Matches(@"\d")
            .WithMessage("A senha deve conter pelo menos um número.")
            .Matches(@"[^a-zA-Z0-9]")
            .WithMessage("A senha deve conter pelo menos um caractere especial.");
    }
}
