namespace Board.Application.Features.Users.CreateUser;

public sealed record CreateUserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTimeOffset CreatedAt
);