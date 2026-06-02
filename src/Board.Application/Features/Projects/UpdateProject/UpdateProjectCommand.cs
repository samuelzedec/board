using System.Text.Json.Serialization;
using Mediator;

namespace Board.Application.Features.Projects.UpdateProject;

public sealed record UpdateProjectCommand(
    [property: JsonIgnore] Guid ProjectId,
    string Name,
    string? Description,
    string Color,
    bool IsActive
) : ICommand<UpdateProjectResponse>;
