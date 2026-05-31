using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Auth.GetMe;

internal sealed class GetMeHandler(
    ICurrentUserService currentUserService,
    IUserRepository userRepository)
    : IQueryHandler<GetMeQuery, GetMeResponse>
{
    public async ValueTask<GetMeResponse> Handle(
        GetMeQuery query,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        return new GetMeResponse(user.Id, user.Name, user.Email, user.CreatedAt);
    }
}
