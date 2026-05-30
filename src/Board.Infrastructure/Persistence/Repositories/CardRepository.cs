using Board.Domain.Entities;
using Board.Domain.Repositories;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class CardRepository(BoardDbContext context)
    : Repository<Card>(context), ICardRepository;