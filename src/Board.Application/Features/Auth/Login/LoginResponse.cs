namespace Board.Application.Features.Auth.Login;

public sealed record LoginResponse(
    string Token,
    DateTimeOffset ExpiresAt
);
