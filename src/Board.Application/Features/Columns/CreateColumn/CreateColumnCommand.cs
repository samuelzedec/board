using Mediator;

namespace Board.Application.Features.Columns.CreateColumn;

public sealed record CreateColumnCommand(
    Guid ProjectId,
    string Name,
    int Order
) : ICommand<CreateColumnResponse>;