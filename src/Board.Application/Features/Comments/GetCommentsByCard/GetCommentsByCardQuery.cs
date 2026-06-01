using Mediator;

namespace Board.Application.Features.Comments.GetCommentsByCard;

public sealed record GetCommentsByCardQuery(
    Guid CardId,
    Guid? AuthorId = null,
    string? Search = null
) : IQuery<IReadOnlyList<GetCommentsByCardResponse>>;