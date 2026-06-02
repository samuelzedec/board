using Board.Domain.Enums;

namespace Board.Application.Features.Cards.UpdateCard;

public sealed record UpdateCardResponse(
    Guid Id,
    Guid ColumnId,
    Guid? AssigneeId,
    string Title,
    string? Description,
    int Order,
    DateOnly? DueDate,
    Priority Priority,
    DateTimeOffset UpdatedAt,
    DateTimeOffset CreatedAt
);