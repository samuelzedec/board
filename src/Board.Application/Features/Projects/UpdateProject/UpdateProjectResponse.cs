namespace Board.Application.Features.Projects.UpdateProject;

public sealed record UpdateProjectResponse(
    Guid Id,
    Guid OwnerId,
    string Name,
    string? Description,
    string Color,
    bool IsActive,
    DateTimeOffset CreatedAt
);
