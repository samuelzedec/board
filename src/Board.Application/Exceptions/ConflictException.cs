using System.Net;

namespace Board.Application.Exceptions;

public sealed class ConflictException(string message)
    : BoardException(message, HttpStatusCode.Conflict);
