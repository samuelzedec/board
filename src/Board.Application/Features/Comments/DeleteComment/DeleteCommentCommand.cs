using Mediator;

namespace Board.Application.Features.Comments.DeleteComment;

public sealed record DeleteCommentCommand(
    Guid CardId,
    Guid CommentId
) : ICommand;