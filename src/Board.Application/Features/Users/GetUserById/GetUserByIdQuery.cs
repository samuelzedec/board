using Mediator;

namespace Board.Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;
