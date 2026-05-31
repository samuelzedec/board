using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Users.GetUsers;

internal sealed class GetUsersHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersQuery, IReadOnlyList<GetUsersResponse>>
{
    public async ValueTask<IReadOnlyList<GetUsersResponse>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return [..users.Select(u => new GetUsersResponse(u.Id, u.Name, u.Email, u.CreatedAt))];
    }
}