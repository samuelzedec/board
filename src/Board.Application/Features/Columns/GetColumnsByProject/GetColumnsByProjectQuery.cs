using Mediator;

namespace Board.Application.Features.Columns.GetColumnsByProject;

public sealed record GetColumnsByProjectQuery(
    Guid ProjectId
) : IQuery<IReadOnlyList<GetColumnsByProjectResponse>>;