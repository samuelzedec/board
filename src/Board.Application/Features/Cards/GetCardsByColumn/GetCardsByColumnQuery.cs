using Mediator;

namespace Board.Application.Features.Cards.GetCardsByColumn;

public sealed record GetCardsByColumnQuery(
    Guid ColumnId
) : IQuery<IReadOnlyList<GetCardsByColumnResponse>>;