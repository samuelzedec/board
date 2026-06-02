using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Columns.GetColumnsByProject;

internal sealed class GetColumnsByProjectHandler(
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetColumnsByProjectQuery, IReadOnlyList<GetColumnsByProjectResponse>>
{
    public async ValueTask<IReadOnlyList<GetColumnsByProjectResponse>> Handle(
        GetColumnsByProjectQuery query,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(query.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode visualizar as colunas.");

        var columns = await columnRepository.GetByProjectIdAsync(query.ProjectId, cancellationToken);

        return columns
            .Select(column => new GetColumnsByProjectResponse(
                column.Id,
                column.ProjectId,
                column.Name,
                column.Order,
                column.CreatedAt))
            .ToList();
    }
}