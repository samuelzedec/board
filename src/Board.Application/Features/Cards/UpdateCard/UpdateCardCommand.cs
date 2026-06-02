using System.Text.Json.Serialization;
using Board.Domain.Enums;
using Mediator;

namespace Board.Application.Features.Cards.UpdateCard;

public sealed record UpdateCardCommand(
    [property: JsonIgnore] Guid CardId,
    string Title,
    string? Description,
    Guid? AssigneeId,
    DateOnly? DueDate,
    Priority Priority = Priority.None
) : ICommand<UpdateCardResponse>;