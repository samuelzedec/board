namespace Board.Application.Features.Comments.CreateComment;

public sealed record CreateCommentResponse(
    Guid Id,
    Guid CardId,
    Guid AuthorId,
    string Content,
    DateTimeOffset? EditedAt,
    DateTimeOffset CreatedAt
);