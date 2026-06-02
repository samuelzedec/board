using Mediator;

namespace Board.Application.Features.Cards.DeleteCard;

public sealed record DeleteCardCommand(
    Guid CardId
) : ICommand;