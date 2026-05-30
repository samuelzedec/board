using Board.Domain.Abstractions;

namespace Board.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    /// <summary>
    /// Adiciona uma nova entidade ao repositório de forma assíncrona.
    /// </summary>
    /// <param name="entity">A entidade a ser adicionada ao repositório.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona, caso necessário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente no repositório de forma assíncrona.
    /// </summary>
    /// <param name="entity">A entidade a ser atualizada no repositório.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona, caso necessário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma entidade existente do repositório de forma assíncrona.
    /// </summary>
    /// <param name="entity">A entidade a ser removida do repositório.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona, caso necessário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma entidade do repositório com base em seu identificador de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único da entidade a ser recuperada.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona, caso necessário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade encontrada ou null caso não seja encontrada.</returns>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}