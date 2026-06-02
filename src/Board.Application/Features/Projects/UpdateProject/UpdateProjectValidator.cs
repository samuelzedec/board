using FluentValidation;

namespace Board.Application.Features.Projects.UpdateProject;

internal sealed class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome do projeto é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do projeto deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("A cor do projeto é obrigatória.")
            .MaximumLength(20)
            .WithMessage("A cor deve ter no máximo 20 caracteres.")
            .Matches("^#[0-9A-Fa-f]{6}$")
            .WithMessage("A cor deve estar no formato hexadecimal válido (Ex: #FFAA33).");
    }
}
