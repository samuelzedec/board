using Board.Domain.Enums;

namespace Board.Application.Features.Cards.CreateCard;

public sealed record CreateCardResponse(
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