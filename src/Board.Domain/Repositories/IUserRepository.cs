using Board.Domain.Entities;

namespace Board.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Obtém um usuário pelo endereço de e-mail fornecido.
    /// </summary>
    /// <param name="email">O endereço de e-mail do usuário a ser buscado.</param>
    /// <param name="cancellationToken">
    /// Token de cancelamento que pode ser usado para cancelar a operação.
    /// </param>
    /// <returns>
    /// Uma instância de <see cref="User"/> correspondente ao endereço de e-mail fornecido,
    /// ou null se nenhum usuário for encontrado.
    /// </returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os usuários cadastrados no sistema.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token de cancelamento que pode ser usado para cancelar a operação.
    /// </param>
    /// <returns>
    /// Uma lista somente leitura contendo instâncias de <see cref="User"/>.
    /// Retorna uma lista vazia se nenhum usuário for encontrado.
    /// </returns>
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);
}