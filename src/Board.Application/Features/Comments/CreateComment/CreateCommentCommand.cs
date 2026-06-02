using System.Text.Json.Serialization;
using Mediator;

namespace Board.Application.Features.Comments.CreateComment;

public sealed record CreateCommentCommand(
    [property: JsonIgnore] Guid CardId,
    string Content
) : ICommand<CreateCommentResponse>;