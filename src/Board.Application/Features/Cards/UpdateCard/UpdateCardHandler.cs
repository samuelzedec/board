using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.UpdateCard;

internal sealed class UpdateCardHandler(
    ICardRepository cardRepository,
    IUserRepository userRepository)
    : ICommandHandler<UpdateCardCommand, UpdateCardResponse>
{
    public async ValueTask<UpdateCardResponse> Handle(
        UpdateCardCommand command,
        CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(command.CardId, cancellationToken);

        if (card is null)
            throw new NotFoundException("Card não encontrado.");

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

    private async Task EnsureAssigneeExistsAsync(Guid? assigneeId, CancellationToken cancellationToken)
    {
        if (assigneeId is null)
            return;

        var user = await userRepository.GetByIdAsync(assigneeId.Value, cancellationToken);

        if (user is null)
            throw new NotFoundException("Usuário responsável não encontrado.");
    }
}