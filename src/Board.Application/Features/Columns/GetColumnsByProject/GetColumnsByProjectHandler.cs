using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Columns.GetColumnsByProject;

internal sealed class GetColumnsByProjectHandler(
    IColumnRepository columnRepository,
    IProjectRepository projectRepository)
    : IQueryHandler<GetColumnsByProjectQuery, IReadOnlyList<GetColumnsByProjectResponse>>
{
    public async ValueTask<IReadOnlyList<GetColumnsByProjectResponse>> Handle(
        GetColumnsByProjectQuery query,
        CancellationToken cancellationToken)
    {
        await EnsureProjectExistsAsync(query.ProjectId, cancellationToken);

        var columns = await columnRepository.GetByProjectIdAsync(query.ProjectId, cancellationToken);

        return columns
            .Select(column => new GetColumnsByProjectResponse(
                column.Id,
                column.ProjectId,
                column.Name,
                column.Order))
            .ToList();
    }

    private async Task EnsureProjectExistsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(projectId, cancellationToken);

        if (project is null)
            throw new NotFoundException("Projeto não encontrado.");
    }
}