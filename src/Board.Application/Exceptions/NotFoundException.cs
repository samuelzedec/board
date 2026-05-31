using System.Net;

namespace Board.Application.Exceptions;

public sealed class NotFoundException(string message)
    : BoardException(message, HttpStatusCode.NotFound);
