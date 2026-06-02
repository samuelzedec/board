using Board.Application.Features.Columns.GetColumnsByProject;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/projects/{projectId:guid}/columns")]
public sealed class ColumnController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<GetColumnsByProjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetColumnsByProjectQuery(projectId), cancellationToken);
        return Ok(response);
    }
}