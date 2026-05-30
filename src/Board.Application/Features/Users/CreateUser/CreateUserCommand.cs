using Mediator;

namespace Board.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Email,
    string Password
) : ICommand<CreateUserResponse>;