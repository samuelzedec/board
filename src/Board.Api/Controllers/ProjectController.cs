using Board.Application.Features.Projects.CreateProject;
using Board.Application.Features.Projects.GetProjects;
using Board.Application.Features.Projects.UpdateProject;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public sealed class ProjectController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<GetProjectsResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetProjectsQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType<CreateProjectResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);
        return Created($"api/projects/{response.Id}", response);
    }

    [HttpPut("{projectId:guid}")]
    [ProducesResponseType<UpdateProjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(
        Guid projectId,
        UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            command with { ProjectId = projectId },
            cancellationToken);
        return Ok(response);
    }
}
