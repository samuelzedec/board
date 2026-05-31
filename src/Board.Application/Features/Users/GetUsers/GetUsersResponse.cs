namespace Board.Application.Features.Users.GetUsers;

public sealed record GetUsersResponse(
    Guid Id,
    string Name,
    string Email,
    DateTimeOffset CreatedAt
);
