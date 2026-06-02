using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.GetCardsByColumn;

internal sealed class GetCardsByColumnHandler(
    ICardRepository cardRepository,
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetCardsByColumnQuery, IReadOnlyList<GetCardsByColumnResponse>>
{
    public async ValueTask<IReadOnlyList<GetCardsByColumnResponse>> Handle(
        GetCardsByColumnQuery query,
        CancellationToken cancellationToken)
    {
        var column = await columnRepository.GetByIdAsync(query.ColumnId, cancellationToken)
            ?? throw new NotFoundException("Coluna não encontrada.");

        await EnsureCurrentUserOwnsProjectAsync(column.ProjectId, cancellationToken);

        var cards = await cardRepository.GetByColumnIdAsync(query.ColumnId, cancellationToken);

        return cards
            .Select(card => new GetCardsByColumnResponse(
                card.Id,
                card.ColumnId,
                card.AssigneeId,
                card.Title,
                card.Description,
                card.Order,
                card.DueDate,
                card.Priority,
                card.UpdatedAt,
                card.CreatedAt))
            .ToList();
    }

    private async Task EnsureCurrentUserOwnsProjectAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(projectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode visualizar os cards.");
    }
}