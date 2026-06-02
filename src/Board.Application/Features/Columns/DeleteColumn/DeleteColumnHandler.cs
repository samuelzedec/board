using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Columns.DeleteColumn;

internal sealed class DeleteColumnHandler(
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<DeleteColumnCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteColumnCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(command.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode remover colunas.");

        var column = await GetColumnAsync(command.ProjectId, command.ColumnId, cancellationToken);

        await columnRepository.DeleteAsync(column, cancellationToken);
        return Unit.Value;
    }

    private async Task<Column> GetColumnAsync(
        Guid projectId,
        Guid columnId,
        CancellationToken cancellationToken)
    {
        var column = await columnRepository.GetByIdAsync(columnId, cancellationToken);

        if (column is null || column.ProjectId != projectId)
            throw new NotFoundException("Coluna não encontrada.");

        return column;
    }
}