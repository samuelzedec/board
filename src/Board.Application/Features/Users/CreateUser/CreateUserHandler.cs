using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Users.CreateUser;

internal sealed class CreateUserHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    public async ValueTask<CreateUserResponse> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        await EnsureEmailIsAvailableAsync(command.Email, cancellationToken);

        var user = MapToEntity(command);
        await userRepository.AddAsync(user, cancellationToken);

        return new CreateUserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.CreatedAt
        );
    }

    private async Task EnsureEmailIsAvailableAsync(string email, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository
            .GetByEmailAsync(email, cancellationToken);

        if (existingUser is not null)
            throw new ConflictException("E-mail já cadastrado.");
    }

    private User MapToEntity(CreateUserCommand command)
        => new() { Name = command.Name, Email = command.Email, PasswordHash = passwordHasher.Hash(command.Password) };
}