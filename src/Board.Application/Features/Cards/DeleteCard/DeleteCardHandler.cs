using Board.Application.Abstractions;
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.DeleteCard;

internal sealed class DeleteCardHandler(
    ICardRepository cardRepository,
    IColumnRepository columnRepository,
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : ICommandHandler<DeleteCardCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteCardCommand command,
        CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(command.CardId, cancellationToken)
            ?? throw new NotFoundException("Card não encontrado.");

        await EnsureCurrentUserOwnsCardProjectAsync(card.ColumnId, cancellationToken);

        await cardRepository.DeleteAsync(card, cancellationToken);
        return Unit.Value;
    }

    private async Task EnsureCurrentUserOwnsCardProjectAsync(Guid columnId, CancellationToken cancellationToken)
    {
        var column = await columnRepository.GetByIdAsync(columnId, cancellationToken)
            ?? throw new NotFoundException("Coluna não encontrada.");

        var project = await projectRepository.GetByIdAsync(column.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Projeto não encontrado.");

        if (project.OwnerId != currentUserService.GetUserId())
            throw new ForbiddenException("Apenas o dono do projeto pode remover o card.");
    }
}