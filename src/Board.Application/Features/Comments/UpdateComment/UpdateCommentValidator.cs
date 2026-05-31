using FluentValidation;

namespace Board.Application.Features.Comments.UpdateComment;

internal sealed class UpdateCommentValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("O conteúdo do comentário é obrigatório.")
            .MaximumLength(2000)
            .WithMessage("O conteúdo do comentário deve ter no máximo 2000 caracteres.");
    }
}