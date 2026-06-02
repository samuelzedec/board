using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Columns.GetColumnById;

internal sealed class GetColumnByIdHandler(
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetColumnByIdQuery, GetColumnByIdResponse>
{
    public async ValueTask<GetColumnByIdResponse> Handle(
        GetColumnByIdQuery query,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(query.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode visualizar esta coluna.");

        var column = await columnRepository.GetByIdAsync(query.ColumnId, cancellationToken);

        if (column is null || column.ProjectId != query.ProjectId)
            throw new NotFoundException("Coluna não encontrada.");

        return new GetColumnByIdResponse(
            column.Id,
            column.ProjectId,
            column.Name,
            column.Order,
            column.CreatedAt
        );
    }
}