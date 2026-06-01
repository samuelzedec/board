using Mediator;

namespace Board.Application.Features.Projects.CreateProject;

public sealed record CreateProjectCommand(
    Guid OwnerId,
    string Name,
    string? Description,
    string Color
) : ICommand<CreateProjectResponse>;