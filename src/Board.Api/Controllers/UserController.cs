using Board.Application.Features.Users.CreateUser;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UserController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);
        return Created($"api/users/{response.Id}", response);
    }
}