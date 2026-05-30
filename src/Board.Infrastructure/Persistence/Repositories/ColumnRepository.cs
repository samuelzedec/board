using Board.Domain.Entities;
using Board.Domain.Repositories;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class ColumnRepository(BoardDbContext context)
    : Repository<Column>(context), IColumnRepository;
