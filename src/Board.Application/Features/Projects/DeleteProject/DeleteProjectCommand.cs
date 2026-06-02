using Mediator;

namespace Board.Application.Features.Projects.DeleteProject;

public sealed record DeleteProjectCommand(Guid ProjectId) : ICommand;
