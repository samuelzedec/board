namespace Board.Application.Features.Comments.UpdateComment;

public sealed record UpdateCommentResponse(
    Guid Id,
    Guid CardId,
    Guid AuthorId,
    string Content,
    DateTimeOffset? EditedAt,
    DateTimeOffset CreatedAt
);