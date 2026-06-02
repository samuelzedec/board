using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Cards.DeleteCard;

internal sealed class DeleteCardHandler(ICardRepository cardRepository)
    : ICommandHandler<DeleteCardCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteCardCommand command,
        CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByIdAsync(command.CardId, cancellationToken);

        if (card is null)
            throw new NotFoundException("Card não encontrado.");

        await cardRepository.DeleteAsync(card, cancellationToken);
        return Unit.Value;
    }
}