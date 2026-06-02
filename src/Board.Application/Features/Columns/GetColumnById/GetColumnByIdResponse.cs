namespace Board.Application.Features.Columns.GetColumnById;

public sealed record GetColumnByIdResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    int Order,
    DateTimeOffset CreatedAt
);