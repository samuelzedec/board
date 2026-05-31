using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Comments.UpdateComment;

internal sealed class UpdateCommentHandler(
    ICommentRepository commentRepository,
    ICardRepository cardRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<UpdateCommentCommand, UpdateCommentResponse>
{
    public async ValueTask<UpdateCommentResponse> Handle(
        UpdateCommentCommand command,
        CancellationToken cancellationToken)
    {
        await EnsureCardExistsAsync(command.CardId, cancellationToken);
        var comment = await GetCommentAsync(command.CardId, command.CommentId, cancellationToken);
        EnsureCurrentUserIsAuthor(comment);

        comment.Content = command.Content;
        comment.EditedAt = DateTimeOffset.UtcNow;
        await commentRepository.UpdateAsync(comment, cancellationToken);

        return new UpdateCommentResponse(
            comment.Id,
            comment.CardId,
            comment.AuthorId,
            comment.Content,
            comment.EditedAt,
            comment.CreatedAt
        );
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
            throw new ForbiddenException("Apenas o autor pode editar o comentário.");
    }
}