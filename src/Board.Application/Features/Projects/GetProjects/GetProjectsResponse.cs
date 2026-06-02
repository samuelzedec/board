namespace Board.Application.Features.Projects.GetProjects;

public sealed record GetProjectsResponse(
    Guid Id,
    Guid OwnerId,
    string Name,
    string? Description,
    string Color,
    bool IsActive,
    DateTimeOffset CreatedAt
);
