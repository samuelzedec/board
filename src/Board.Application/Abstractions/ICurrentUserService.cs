namespace Board.Application.Abstractions;

/// <summary>
/// Interface responsável por fornecer informações sobre o usuário atualmente autenticado no sistema.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Retorna o identificador único (UUID) do usuário atual.
    /// </summary>
    /// <returns>O identificador único (UUID) do usuário atual.</returns>
    Guid GetUserId();

    /// <summary>
    /// Retorna o endereço de e-mail do usuário atual.
    /// </summary>
    /// <returns>O endereço de e-mail do usuário atual.</returns>
    string GetEmail();
}