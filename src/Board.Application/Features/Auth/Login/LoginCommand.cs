using Mediator;

namespace Board.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : ICommand<LoginResponse>;
