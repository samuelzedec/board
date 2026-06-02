using Board.Application.Features.Columns.CreateColumn;
using Board.Application.Features.Columns.DeleteColumn;
using Board.Application.Features.Columns.GetColumnById;
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
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        Guid projectId,
        CreateColumnCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(command with { ProjectId = projectId }, cancellationToken);
        return Created($"api/projects/{projectId}/columns/{response.Id}", response);
    }

    [HttpGet]
    public async Task<IActionResult> GetByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetColumnsByProjectQuery(projectId), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{columnId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        Guid projectId,
        Guid columnId,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetColumnByIdQuery(projectId, columnId), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{columnId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        Guid projectId,
        Guid columnId,
        CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteColumnCommand(projectId, columnId), cancellationToken);
        return NoContent();
    }
}