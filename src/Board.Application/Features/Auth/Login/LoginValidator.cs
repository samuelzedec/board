using FluentValidation;

namespace Board.Application.Features.Auth.Login;

internal sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
            .WithMessage("O e-mail informado não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.");
    }
}
