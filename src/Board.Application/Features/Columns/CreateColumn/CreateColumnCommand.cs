using System.Text.Json.Serialization;
using Mediator;

namespace Board.Application.Features.Columns.CreateColumn;

public sealed record CreateColumnCommand(
    [property: JsonIgnore] Guid ProjectId,
    string Name,
    int Order
) : ICommand<CreateColumnResponse>;