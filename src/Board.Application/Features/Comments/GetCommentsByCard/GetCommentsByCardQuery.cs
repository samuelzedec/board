using Mediator;

namespace Board.Application.Features.Comments.GetCommentsByCard;

public sealed record GetCommentsByCardQuery(
    Guid CardId
) : IQuery<IReadOnlyList<GetCommentsByCardResponse>>;