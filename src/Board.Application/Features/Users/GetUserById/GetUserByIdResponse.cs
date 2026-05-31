namespace Board.Application.Features.Users.GetUserById;

public sealed record GetUserByIdResponse(
    Guid Id,
    string Name,
    string Email,
    DateTimeOffset CreatedAt
);
