using Board.Domain.Enums;
using Mediator;

namespace Board.Application.Features.Cards.UpdateCard;

public sealed record UpdateCardCommand(
    Guid CardId,
    string Title,
    string? Description,
    Guid? AssigneeId,
    DateOnly? DueDate,
    Priority Priority = Priority.None
) : ICommand<UpdateCardResponse>;