using Mediator;

namespace Board.Application.Features.Cards.GetCardById;

public sealed record GetCardByIdQuery(
    Guid CardId
) : IQuery<GetCardByIdResponse>;