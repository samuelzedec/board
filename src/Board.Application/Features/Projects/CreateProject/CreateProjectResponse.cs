namespace Board.Application.Features.Projects.CreateProject;

public sealed record CreateProjectResponse(
    Guid Id,
    Guid OwnerId,
    string Name,
    string? Description,
    string Color,
    bool IsActive,
    DateTimeOffset CreatedAt
);