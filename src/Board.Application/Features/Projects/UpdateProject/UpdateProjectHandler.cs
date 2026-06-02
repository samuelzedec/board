using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Projects.UpdateProject;

internal sealed class UpdateProjectHandler(
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<UpdateProjectCommand, UpdateProjectResponse>
{
    public async ValueTask<UpdateProjectResponse> Handle(
        UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(command.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode atualizá-lo.");

        await EnsureNameIsAvailableAsync(command.Name, project.OwnerId, project.Id, cancellationToken);

        project.Name = command.Name;
        project.Description = command.Description;
        project.Color = command.Color;
        project.IsActive = command.IsActive;

        await projectRepository.UpdateAsync(project, cancellationToken);

        return new UpdateProjectResponse(
            project.Id,
            project.OwnerId,
            project.Name,
            project.Description,
            project.Color,
            project.IsActive,
            project.CreatedAt
        );
    }

    private async Task EnsureNameIsAvailableAsync(
        string name,
        Guid ownerId,
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var existingProject = await projectRepository
            .GetByNameAndOwnerAsync(name, ownerId, cancellationToken);

        if (existingProject is not null && existingProject.Id != projectId)
            throw new ConflictException("Você já é dono de um projeto com esse nome.");
    }
}
