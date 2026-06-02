using System.Text.Json.Serialization;
using Mediator;

namespace Board.Application.Features.Comments.UpdateComment;

public sealed record UpdateCommentCommand(
    [property: JsonIgnore] Guid CardId,
    [property: JsonIgnore] Guid CommentId,
    string Content
) : ICommand<UpdateCommentResponse>;