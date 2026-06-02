using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Projects.DeleteProject;

internal sealed class DeleteProjectHandler(
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<DeleteProjectCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(command.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode removê-lo.");

        await projectRepository.DeleteAsync(project, cancellationToken);
        return Unit.Value;
    }
}
