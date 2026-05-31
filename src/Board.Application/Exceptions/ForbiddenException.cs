using System.Net;

namespace Board.Application.Exceptions;

public sealed class ForbiddenException(string message)
    : BoardException(message, HttpStatusCode.Forbidden);