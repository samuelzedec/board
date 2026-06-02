using Board.Application.Features.Cards.CreateCard;
using Board.Application.Features.Cards.DeleteCard;
using Board.Application.Features.Cards.GetCardById;
using Board.Application.Features.Cards.GetCardsByColumn;
using Board.Application.Features.Cards.UpdateCard;
using Board.Domain.Enums;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Controllers;

[ApiController]
[Authorize]
public sealed class CardController(ISender sender) : ControllerBase
{
    [HttpPost("api/columns/{columnId:guid}/cards")]
    [ProducesResponseType<CreateCardResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync(
        Guid columnId,
        CreateCardCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(command with { ColumnId = columnId }, cancellationToken);
        return Created($"api/cards/{response.Id}", response);
    }

    [HttpGet("api/columns/{columnId:guid}/cards")]
    [ProducesResponseType<IReadOnlyList<GetCardsByColumnResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByColumnAsync(
        Guid columnId,
        CancellationToken cancellationToken,
        [FromQuery] Guid? assigneeId = null,
        [FromQuery] Priority? priority = null)
    {
        var response = await sender.Send(
            new GetCardsByColumnQuery(columnId, assigneeId, priority),
            cancellationToken);
        return Ok(response);
    }

    [HttpGet("api/cards/{cardId:guid}")]
    [ProducesResponseType<GetCardByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
        Guid cardId,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetCardByIdQuery(cardId), cancellationToken);
        return Ok(response);
    }

    [HttpPut("api/cards/{cardId:guid}")]
    [ProducesResponseType<UpdateCardResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid cardId,
        UpdateCardCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(command with { CardId = cardId }, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("api/cards/{cardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(
        Guid cardId,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteCardCommand(cardId), cancellationToken);
        return NoContent();
    }
}