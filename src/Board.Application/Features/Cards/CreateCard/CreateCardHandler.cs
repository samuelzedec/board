using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.CreateCard;

internal sealed class CreateCardHandler(
    ICardRepository cardRepository,
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<CreateCardCommand, CreateCardResponse>
{
    public async ValueTask<CreateCardResponse> Handle(
        CreateCardCommand command,
        CancellationToken cancellationToken)
    {
        var column = await columnRepository.GetByIdAsync(command.ColumnId, cancellationToken)
            ?? throw new NotFoundException("Coluna não encontrada.");

        await EnsureCurrentUserOwnsProjectAsync(column.ProjectId, cancellationToken);
        await EnsureAssigneeExistsAsync(command.AssigneeId, cancellationToken);

        var nextOrder = await GetNextOrderAsync(command.ColumnId, cancellationToken);

        var card = new Card
        {
            ColumnId = command.ColumnId,
            AssigneeId = command.AssigneeId,
            Title = command.Title,
            Description = command.Description,
            Order = nextOrder,
            DueDate = command.DueDate,
            Priority = command.Priority,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await cardRepository.AddAsync(card, cancellationToken);

        return new CreateCardResponse(
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

    private async Task EnsureCurrentUserOwnsProjectAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(projectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode gerenciar os cards.");
    }

    private async Task EnsureAssigneeExistsAsync(Guid? assigneeId, CancellationToken cancellationToken)
    {
        if (assigneeId is null)
            return;

        var user = await userRepository.GetByIdAsync(assigneeId.Value, cancellationToken);

        if (user is null)
            throw new NotFoundException("Usuário responsável não encontrado.");
    }

    private async Task<int> GetNextOrderAsync(Guid columnId, CancellationToken cancellationToken)
    {
        var cards = await cardRepository.GetByColumnIdAsync(columnId, cancellationToken);
        return cards.Count == 0 ? 0 : cards.Max(c => c.Order) + 1;
    }
}