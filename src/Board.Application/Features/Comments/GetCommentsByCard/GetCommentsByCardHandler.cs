using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Comments.GetCommentsByCard;

internal sealed class GetCommentsByCardHandler(
    ICommentRepository commentRepository,
    ICardRepository cardRepository)
    : IQueryHandler<GetCommentsByCardQuery, IReadOnlyList<GetCommentsByCardResponse>>
{
    public async ValueTask<IReadOnlyList<GetCommentsByCardResponse>> Handle(
        GetCommentsByCardQuery query,
        CancellationToken cancellationToken)
    {
        await EnsureCardExistsAsync(query.CardId, cancellationToken);

        var comments = await commentRepository.GetByCardIdAsync(
            query.CardId,
            query.AuthorId,
            query.Search,
            cancellationToken);

        return comments
            .Select(comment => new GetCommentsByCardResponse(
                comment.Id,
                comment.CardId,
                comment.AuthorId,
                comment.Content,
                comment.EditedAt,
                comment.CreatedAt))
            .ToList();
    }

    private async Task EnsureCardExistsAsync(Guid cardId, CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(cardId, cancellationToken);

        if (card is null)
            throw new NotFoundException("Card não encontrado.");
    }
}