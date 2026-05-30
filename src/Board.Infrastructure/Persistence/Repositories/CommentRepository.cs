using Board.Domain.Entities;
using Board.Domain.Repositories;

namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class CommentRepository(BoardDbContext context)
    : Repository<Comment>(context), ICommentRepository;