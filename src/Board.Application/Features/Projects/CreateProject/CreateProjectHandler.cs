using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Projects.CreateProject;

internal sealed class CreateProjectHandler(
    IProjectRepository projectRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<CreateProjectCommand, CreateProjectResponse>
{
    public async ValueTask<CreateProjectResponse> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var ownerId = currentUserService.GetUserId();

        await EnsureOwnerExistsAsync(ownerId, cancellationToken);
        await EnsureNameIsAvailableAsync(command.Name, ownerId, cancellationToken);

        var project = MapToEntity(command, ownerId);
        await projectRepository.AddAsync(project, cancellationToken);

        return new CreateProjectResponse(
            project.Id,
            project.OwnerId,
            project.Name,
            project.Description,
            project.Color,
            project.IsActive,
            project.CreatedAt
        );
    }

    private async Task EnsureOwnerExistsAsync(Guid ownerId, CancellationToken cancellationToken)
    {
        var owner = await userRepository.GetByIdAsync(ownerId, cancellationToken);

        if (owner is null)
            throw new NotFoundException("Usuário não encontrado.");
    }

    private static Project MapToEntity(CreateProjectCommand command, Guid ownerId)
        => new()
        {
            Name = command.Name,
            Description = command.Description,
            Color = command.Color,
            OwnerId = ownerId,
            Columns =
            [
                new Column { Name = "A Fazer", Order = 0 },
                new Column { Name = "Em Progresso", Order = 1 },
                new Column { Name = "Concluído", Order = 2 }
            ]
        };

    private async Task EnsureNameIsAvailableAsync(
        string name,
        Guid ownerId,
        CancellationToken cancellationToken)
    {
        var existingProject = await projectRepository
            .GetByNameAndOwnerAsync(name, ownerId, cancellationToken);

        if (existingProject is not null)
            throw new ConflictException("Você já é dono de um projeto com esse nome.");
    }
}
