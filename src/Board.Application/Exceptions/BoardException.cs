using System.Net;

namespace Board.Application.Exceptions;


public abstract class BoardException(string message, HttpStatusCode statusCode)
    : Exception(message)
{
    public HttpStatusCode StatusCode => statusCode;
}