using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Auth.Login;

internal sealed class LoginHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async ValueTask<LoginResponse> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null || !passwordHasher.Verify(command.Password, user.PasswordHash))
            throw new UnauthorizedException("Credenciais inválidas.");

        var tokenResult = tokenService.GenerateToken(user);
        return new LoginResponse(tokenResult.Token, tokenResult.ExpiresAt);
    }
}
