using FluentValidation;

namespace Board.Application.Features.Comments.CreateComment;

internal sealed class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("O conteúdo do comentário é obrigatório.")
            .MaximumLength(2000)
            .WithMessage("O conteúdo do comentário deve ter no máximo 2000 caracteres.");
    }
}