using System.Text.Json.Serialization;
using Board.Domain.Enums;
using Mediator;

namespace Board.Application.Features.Cards.CreateCard;

public sealed record CreateCardCommand(
    [property: JsonIgnore] Guid ColumnId,
    string Title,
    string? Description,
    Guid? AssigneeId,
    DateOnly? DueDate,
    Priority Priority = Priority.None
) : ICommand<CreateCardResponse>;