using FluentValidation;

namespace Board.Application.Features.Columns.CreateColumn;

internal sealed class CreateColumnValidator : AbstractValidator<CreateColumnCommand>
{
    public CreateColumnValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome da coluna é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome da coluna deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(1)
            .WithMessage("A ordem da coluna deve ser maior ou igual a 1.");
    }
}