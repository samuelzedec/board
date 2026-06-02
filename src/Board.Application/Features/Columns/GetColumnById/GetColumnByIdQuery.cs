using Mediator;

namespace Board.Application.Features.Columns.GetColumnById;

public sealed record GetColumnByIdQuery(
    Guid ProjectId,
    Guid ColumnId
) : IQuery<GetColumnByIdResponse>;