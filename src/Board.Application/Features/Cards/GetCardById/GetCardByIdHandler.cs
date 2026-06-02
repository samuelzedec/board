using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.GetCardById;

internal sealed class GetCardByIdHandler(ICardRepository cardRepository)
    : IQueryHandler<GetCardByIdQuery, GetCardByIdResponse>
{
    public async ValueTask<GetCardByIdResponse> Handle(
        GetCardByIdQuery query,
        CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(query.CardId, cancellationToken);

        if (card is null)
            throw new NotFoundException("Card não encontrado.");

        return new GetCardByIdResponse(
            card.Id,
            card.ColumnId,
            card.AssigneeId,
            card.Title,
            card.Description,
            card.Order,
            card.DueDate,
            card.Priority,
            card.UpdatedAt,
            card.CreatedAt);
    }
}