using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Users.GetUserById;

internal sealed class GetUserByIdHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async ValueTask<GetUserByIdResponse> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        return new GetUserByIdResponse(user.Id, user.Name, user.Email, user.CreatedAt);
    }
}
