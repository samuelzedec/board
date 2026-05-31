using Mediator;

namespace Board.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery : IQuery<IReadOnlyList<GetUsersResponse>>;
