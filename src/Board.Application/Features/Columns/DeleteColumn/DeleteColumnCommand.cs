using Mediator;

namespace Board.Application.Features.Columns.DeleteColumn;

public sealed record DeleteColumnCommand(
    Guid ProjectId,
    Guid ColumnId
) : ICommand;