using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.GetCardsByColumn;

internal sealed class GetCardsByColumnHandler(
    ICardRepository cardRepository,
    IColumnRepository columnRepository)
    : IQueryHandler<GetCardsByColumnQuery, IReadOnlyList<GetCardsByColumnResponse>>
{
    public async ValueTask<IReadOnlyList<GetCardsByColumnResponse>> Handle(
        GetCardsByColumnQuery query,
        CancellationToken cancellationToken)
    {
        await EnsureColumnExistsAsync(query.ColumnId, cancellationToken);

        var cards = await cardRepository.GetByColumnIdAsync(query.ColumnId, cancellationToken);

        return cards
            .Select(card => new GetCardsByColumnResponse(
                card.Id,
                card.ColumnId,
                card.AssigneeId,
                card.Title,
                card.Description,
                card.Order,
                card.DueDate,
                card.Priority,
                card.UpdatedAt,
                card.CreatedAt))
            .ToList();
    }

    private async Task EnsureColumnExistsAsync(Guid columnId, CancellationToken cancellationToken)
    {
        var column = await columnRepository.GetByIdAsync(columnId, cancellationToken);

        if (column is null)
            throw new NotFoundException("Coluna não encontrada.");
    }
}