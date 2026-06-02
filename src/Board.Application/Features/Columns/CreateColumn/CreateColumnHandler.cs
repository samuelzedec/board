using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Columns.CreateColumn;

internal sealed class CreateColumnHandler(
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<CreateColumnCommand, CreateColumnResponse>
{
    public async ValueTask<CreateColumnResponse> Handle(
        CreateColumnCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(command.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode criar colunas.");

        var existingColumn = await columnRepository.GetByProjectIdAndOrderAsync(
            command.ProjectId,
            command.Order,
            cancellationToken);

        if (existingColumn is not null)
            throw new ConflictException("Já existe uma coluna com essa ordem nesse projeto.");

        var column = new Column
        {
            ProjectId = command.ProjectId,
            Name = command.Name,
            Order = command.Order
        };

        await columnRepository.AddAsync(column, cancellationToken);

        return new CreateColumnResponse(
            column.Id,
            column.ProjectId,
            column.Name,
            column.Order,
            column.CreatedAt
        );
    }
}