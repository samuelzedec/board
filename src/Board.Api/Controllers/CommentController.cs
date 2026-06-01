using Board.Application.Features.Comments.CreateComment;
using Board.Application.Features.Comments.DeleteComment;
using Board.Application.Features.Comments.GetCommentsByCard;
using Board.Application.Features.Comments.UpdateComment;
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

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<GetCommentsByCardResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCardAsync(
        Guid cardId,
        CancellationToken cancellationToken,
        [FromQuery] Guid? authorId = null,
        [FromQuery] string? search = null)
    {
        var response = await sender.Send(
            new GetCommentsByCardQuery(cardId, authorId, search),
            cancellationToken);
        return Ok(response);
    }

    [HttpPut("{commentId:guid}")]
    [ProducesResponseType<UpdateCommentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid cardId,
        Guid commentId,
        UpdateCommentCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            command with { CardId = cardId, CommentId = commentId },
            cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        Guid cardId,
        Guid commentId,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteCommentCommand(cardId, commentId), cancellationToken);
        return NoContent();
    }
}