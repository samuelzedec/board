using Mediator;

namespace Board.Application.Features.Comments.CreateComment;

public sealed record CreateCommentCommand(
    Guid CardId,
    string Content
) : ICommand<CreateCommentResponse>;