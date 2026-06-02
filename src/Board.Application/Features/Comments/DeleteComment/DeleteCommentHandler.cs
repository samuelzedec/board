using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Comments.DeleteComment;

internal sealed class DeleteCommentHandler(
    ICommentRepository commentRepository,
    ICardRepository cardRepository,
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<DeleteCommentCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteCommentCommand command,
        CancellationToken cancellationToken)
    {
        var card = await GetCardAsync(command.CardId, cancellationToken);
        var comment = await GetCommentAsync(command.CardId, command.CommentId, cancellationToken);

        await EnsureCanDeleteAsync(comment, card.ColumnId, cancellationToken);

        await commentRepository.DeleteAsync(comment, cancellationToken);
        return Unit.Value;
    }

    private async Task<Card> GetCardAsync(Guid cardId, CancellationToken cancellationToken)
        => await cardRepository.GetByIdAsync(cardId, cancellationToken)
            ?? throw new NotFoundException("Card não encontrado.");

    private async Task<Comment> GetCommentAsync(Guid cardId, Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(commentId, cancellationToken);

        if (comment is null || comment.CardId != cardId)
            throw new NotFoundException("Comentário não encontrado.");

        return comment;
    }

    private async Task EnsureCanDeleteAsync(Comment comment, Guid columnId, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        // O autor sempre pode remover o próprio comentário.
        if (comment.AuthorId == userId)
            return;

        // Caso contrário, só o dono do projeto pode remover (moderação).
        var column = await columnRepository.GetByIdAsync(columnId, cancellationToken)
            ?? throw new NotFoundException("Coluna não encontrada.");

        var project = await projectRepository.GetByIdAsync(column.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != userId)
            throw new ForbiddenException("Apenas o autor ou o dono do projeto pode remover o comentário.");
    }
}