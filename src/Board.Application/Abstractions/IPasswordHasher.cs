namespace Board.Application.Abstractions;

/// <summary>
/// Define a funcionalidade para geração e verificação de hashes de senhas.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Gera um hash para a senha fornecida.
    /// </summary>
    /// <param name="password">A senha em texto puro que será transformada em hash.</param>
    /// <returns>Uma string representando o hash da senha fornecida.</returns>
    string Hash(string password);

    /// <summary>
    /// Verifica se a senha fornecida corresponde ao hash da senha armazenado.
    /// </summary>
    /// <param name="password">A senha em texto puro que será verificada.</param>
    /// <param name="passwordHash">O hash da senha armazenado para comparação.</param>
    /// <returns>Um valor booleano indicando se a senha fornecida corresponde ao hash armazenado.</returns>
    bool Verify(string password, string passwordHash);
}
