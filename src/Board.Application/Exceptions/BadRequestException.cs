using System.Net;

namespace Board.Application.Exceptions;

/// <summary>
/// Exceção lançada quando uma ou mais regras de validação são violadas,
/// encapsulando os erros agrupados por campo para facilitar o retorno estruturado ao cliente.
/// </summary>
public sealed class BadRequestException(Dictionary<string, string[]>? errors)
    : BoardException("Dados inválidos.", HttpStatusCode.BadRequest)
{
    public Dictionary<string, string[]>? Errors => errors;
}
