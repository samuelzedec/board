using System.Net;

namespace Board.Application.Exceptions;

public sealed class UnauthorizedException(string message)
    : BoardException(message, HttpStatusCode.Unauthorized);
