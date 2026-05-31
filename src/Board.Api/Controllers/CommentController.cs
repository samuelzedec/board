using Board.Application.Features.Comments.CreateComment;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/cards/{cardId:guid}/comments")]
public sealed class CommentController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateCommentResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync(
        Guid cardId,
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(command with { CardId = cardId }, cancellationToken);
        return Created($"api/cards/{cardId}/comments/{response.Id}", response);
    }
}