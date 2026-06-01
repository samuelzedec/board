using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Comments.DeleteComment;

internal sealed class DeleteCommentHandler(
    ICommentRepository commentRepository,
    ICardRepository cardRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<DeleteCommentCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteCommentCommand command,
        CancellationToken cancellationToken)
    {
        await EnsureCardExistsAsync(command.CardId, cancellationToken);
        var comment = await GetCommentAsync(command.CardId, command.CommentId, cancellationToken);
        EnsureCurrentUserIsAuthor(comment);

        await commentRepository.DeleteAsync(comment, cancellationToken);
        return Unit.Value;
    }

    private async Task EnsureCardExistsAsync(Guid cardId, CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(cardId, cancellationToken);

        if (card is null)
            throw new NotFoundException("Card não encontrado.");
    }

    private async Task<Comment> GetCommentAsync(Guid cardId, Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(commentId, cancellationToken);

        if (comment is null || comment.CardId != cardId)
            throw new NotFoundException("Comentário não encontrado.");

        return comment;
    }

    private void EnsureCurrentUserIsAuthor(Comment comment)
    {
        if (comment.AuthorId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o autor pode remover o comentário.");
    }
}