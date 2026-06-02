using Mediator;

namespace Board.Application.Features.Projects.GetProjects;

public sealed record GetProjectsQuery : IQuery<IReadOnlyList<GetProjectsResponse>>;
