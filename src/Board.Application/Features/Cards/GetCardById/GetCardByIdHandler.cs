using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.GetCardById;

internal sealed class GetCardByIdHandler(
    ICardRepository cardRepository,
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetCardByIdQuery, GetCardByIdResponse>
{
    public async ValueTask<GetCardByIdResponse> Handle(
        GetCardByIdQuery query,
        CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(query.CardId, cancellationToken)
            ?? throw new NotFoundException("Card não encontrado.");

        await EnsureCurrentUserOwnsCardProjectAsync(card.ColumnId, cancellationToken);

        return new GetCardByIdResponse(
            card.Id,
            card.ColumnId,
            card.AssigneeId,
            card.Title,
            card.Description,
            card.Order,
            card.DueDate,
            card.Priority,
            card.UpdatedAt,
            card.CreatedAt);
    }

    private async Task EnsureCurrentUserOwnsCardProjectAsync(Guid columnId, CancellationToken cancellationToken)
    {
        var column = await columnRepository.GetByIdAsync(columnId, cancellationToken)
            ?? throw new NotFoundException("Coluna não encontrada.");

        var project = await projectRepository.GetByIdAsync(column.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode visualizar o card.");
    }
}