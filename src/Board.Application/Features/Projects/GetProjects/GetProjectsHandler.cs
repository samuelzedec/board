using Board.Application.Abstractions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Projects.GetProjects;

internal sealed class GetProjectsHandler(
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetProjectsQuery, IReadOnlyList<GetProjectsResponse>>
{
    public async ValueTask<IReadOnlyList<GetProjectsResponse>> Handle(
        GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var projects = await projectRepository.GetByOwnerAsync(
            currentUserService.GetUserId(),
            cancellationToken);

        return projects
            .Select(project => new GetProjectsResponse(
                project.Id,
                project.OwnerId,
                project.Name,
                project.Description,
                project.Color,
                project.IsActive,
                project.CreatedAt
            ))
            .ToList();
    }
}
