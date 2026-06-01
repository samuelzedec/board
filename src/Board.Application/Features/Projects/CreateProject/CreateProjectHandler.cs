using Board.Application.Exceptions;
using Board.Domain.Entities;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Projects.CreateProject;

internal sealed class CreateProjectHandler(
    IProjectRepository projectRepository,
    IUserRepository userRepository)
    : ICommandHandler<CreateProjectCommand, CreateProjectResponse>
{
    public async ValueTask<CreateProjectResponse> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        await EnsureOwnerExistsAsync(command.OwnerId, cancellationToken);
        await EnsureNameIsAvailableAsync(command.Name, command.OwnerId, cancellationToken);

        var project = MapToEntity(command);

        project.Columns =
        [
            new Column { Name = "A Fazer", Order = 0 },
            new Column { Name = "Em Progresso", Order = 1 },
            new Column { Name = "Concluído", Order = 2 }
        ];

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

    private static Project MapToEntity(CreateProjectCommand command)
        => new()
        {
            Name = command.Name,
            Description = command.Description,
            Color = command.Color,
            OwnerId = command.OwnerId
        };
}
