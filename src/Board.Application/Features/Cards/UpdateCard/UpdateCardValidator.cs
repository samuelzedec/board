using FluentValidation;

namespace Board.Application.Features.Cards.UpdateCard;

internal sealed class UpdateCardValidator : AbstractValidator<UpdateCardCommand>
{
    public UpdateCardValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("O título do card é obrigatório.")
            .MaximumLength(255)
            .WithMessage("O título deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("A descrição deve ter no máximo 2000 caracteres.")
            .When(x => x.Description is not null);

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Prioridade inválida.");
    }
}