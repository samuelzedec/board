using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Comments.CreateComment;

internal sealed class CreateCommentHandler(
    ICommentRepository commentRepository,
    ICardRepository cardRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<CreateCommentCommand, CreateCommentResponse>
{
    public async ValueTask<CreateCommentResponse> Handle(
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        await EnsureCardExistsAsync(command.CardId, cancellationToken);

        var comment = new Comment
        {
            CardId = command.CardId,
            AuthorId = currentUserService.GetUserId(),
            Content = command.Content
        };

        await commentRepository.AddAsync(comment, cancellationToken);

        return new CreateCommentResponse(
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
}