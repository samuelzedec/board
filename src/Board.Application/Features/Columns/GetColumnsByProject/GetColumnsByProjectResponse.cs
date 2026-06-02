namespace Board.Application.Features.Columns.GetColumnsByProject;

public sealed record GetColumnsByProjectResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    int Order
);