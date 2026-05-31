using Mediator;

namespace Board.Application.Features.Comments.UpdateComment;

public sealed record UpdateCommentCommand(
    Guid CardId,
    Guid CommentId,
    string Content
) : ICommand<UpdateCommentResponse>;