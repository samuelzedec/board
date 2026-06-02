using Board.Domain.Enums;
using Mediator;

namespace Board.Application.Features.Cards.CreateCard;

public sealed record CreateCardCommand(
    Guid ColumnId,
    string Title,
    string? Description,
    Guid? AssigneeId,
    DateOnly? DueDate,
    Priority Priority = Priority.None
) : ICommand<CreateCardResponse>;