using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.UpdateCard;

internal sealed class UpdateCardHandler(
    ICardRepository cardRepository,
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<UpdateCardCommand, UpdateCardResponse>
{
    public async ValueTask<UpdateCardResponse> Handle(
        UpdateCardCommand command,
        CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(command.CardId, cancellationToken)
            ?? throw new NotFoundException("Card não encontrado.");

        await EnsureCurrentUserOwnsCardProjectAsync(card.ColumnId, cancellationToken);
        await EnsureAssigneeExistsAsync(command.AssigneeId, cancellationToken);

        card.Title = command.Title;
        card.Description = command.Description;
        card.AssigneeId = command.AssigneeId;
        card.DueDate = command.DueDate;
        card.Priority = command.Priority;
        card.UpdatedAt = DateTimeOffset.UtcNow;

        await cardRepository.UpdateAsync(card, cancellationToken);

        return new UpdateCardResponse(
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
            throw new ForbiddenException("Apenas o dono do projeto pode atualizar o card.");
    }

    private async Task EnsureAssigneeExistsAsync(Guid? assigneeId, CancellationToken cancellationToken)
    {
        if (assigneeId is null)
            return;

        var user = await userRepository.GetByIdAsync(assigneeId.Value, cancellationToken);

        if (user is null)
            throw new NotFoundException("Usuário responsável não encontrado.");
    }
}