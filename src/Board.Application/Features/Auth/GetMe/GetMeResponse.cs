namespace Board.Application.Features.Auth.GetMe;

public sealed record GetMeResponse(
    Guid Id,
    string Name,
    string Email,
    DateTimeOffset CreatedAt
);
