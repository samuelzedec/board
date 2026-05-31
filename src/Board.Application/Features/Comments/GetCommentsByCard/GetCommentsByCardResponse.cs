namespace Board.Application.Features.Comments.GetCommentsByCard;

public sealed record GetCommentsByCardResponse(
    Guid Id,
    Guid CardId,
    Guid AuthorId,
    string Content,
    DateTimeOffset? EditedAt,
    DateTimeOffset CreatedAt
);