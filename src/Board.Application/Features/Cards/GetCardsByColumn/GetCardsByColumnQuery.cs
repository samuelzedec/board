using Board.Domain.Enums;
using Mediator;

namespace Board.Application.Features.Cards.GetCardsByColumn;

public sealed record GetCardsByColumnQuery(
    Guid ColumnId,
    Guid? AssigneeId = null,
    Priority? Priority = null
) : IQuery<IReadOnlyList<GetCardsByColumnResponse>>;