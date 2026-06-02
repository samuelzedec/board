namespace Board.Application.Features.Columns.CreateColumn;

public sealed record CreateColumnResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    int Order,
    DateTimeOffset CreatedAt
);