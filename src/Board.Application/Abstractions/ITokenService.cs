using Board.Application.DTOs;
using Board.Domain.Entities;

namespace Board.Application.Abstractions;

/// <summary>
/// Interface responsável pelo serviço de geração de tokens JWT.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um token JWT para o usuário especificado.
    /// </summary>
    /// <param name="user">Usuário para o qual o token será gerado.</param>
    /// <returns>O token JWT e sua data de expiração.</returns>
    TokenResult GenerateToken(User user);
}
