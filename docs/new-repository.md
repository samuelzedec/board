# Criando um novo repositório

Passo a passo para adicionar um repositório para uma nova entidade.

---

## Estrutura envolvida

```
src/Board.Domain/
└── Abstractions/
    └── Repositories/
        ├── IRepository.cs          ← contrato genérico (já existe)
        ├── IUnitOfWork.cs          ← adicionar propriedade aqui
        └── IUserRepository.cs      ← novo contrato específico

src/Board.Infrastructure/
└── Persistence/
    ├── Repositories/
    │   ├── Repository.cs           ← implementação base (já existe)
    │   ├── UnitOfWork.cs           ← adicionar propriedade aqui
    │   └── UserRepository.cs       ← nova implementação
    └── Mappings/
        └── UserMap.cs              ← novo mapeamento EF Core
```

---

## 1. Interface do repositório (Domain)

Crie a interface em `Domain/Abstractions/Repositories/` estendendo `IRepository<T>`:

```csharp
// Domain/Abstractions/Repositories/IUserRepository.cs
namespace Board.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
```

- Herde de `IRepository<TEntity>` para ganhar `CreateAsync`, `Update`, `Delete` e `GetByIdAsync`.
- Declare aqui apenas métodos específicos da entidade.

---

## 2. Adicionar ao IUnitOfWork (Domain)

Exponha o repositório no contrato de unidade de trabalho:

```csharp
// Domain/Abstractions/Repositories/IUnitOfWork.cs
public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    ValueTask BeginTransactionAsync(CancellationToken cancellationToken = default);
    ValueTask CommitTransactionAsync(CancellationToken cancellationToken = default);
    ValueTask RollbackTransactionAsync(CancellationToken cancellationToken);
}
```

---

## 3. Implementação do repositório (Infrastructure)

Crie a implementação em `Infrastructure/Persistence/Repositories/` estendendo `Repository<T>`:

```csharp
// Infrastructure/Persistence/Repositories/UserRepository.cs
namespace Board.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(BoardDbContext context)
    : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
        => await _table
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
}
```

- Use `AsNoTracking()` em queries de leitura pura (sem intenção de atualizar a entidade).
- `_table` é o `DbSet<T>` herdado de `Repository<T>`.

---

## 4. Adicionar ao UnitOfWork (Infrastructure)

Exponha o repositório na implementação da unidade de trabalho:

```csharp
// Infrastructure/Persistence/Repositories/UnitOfWork.cs
internal sealed class UnitOfWork(
    BoardDbContext context,
    IMediator mediator)
    : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public IUserRepository Users =>
        field ??= new UserRepository(context);

    // ... restante inalterado
}
```

---

## 5. Mapeamento EF Core (Infrastructure)

Crie o mapeamento em `Infrastructure/Persistence/Mappings/` estendendo `BaseEntityTypeConfiguration<T>`:

```csharp
// Infrastructure/Persistence/Mappings/UserMap.cs
namespace Board.Infrastructure.Persistence.Mappings;

internal sealed class UserMap : BaseEntityTypeConfiguration<User>
{
    protected override string GetTableName() => "users";

    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasColumnType("varchar(20)");
    }
}
```

- `BaseEntityTypeConfiguration<T>` já configura `Id`, `CreatedAt`, `UpdatedAt`, `DeletedAt` e o query filter de soft delete.
- O EF Core descobre o mapeamento automaticamente via `ApplyConfigurationsFromAssembly` no `DbContext`.
- Nenhum registro de DI adicional é necessário — apenas `IUnitOfWork` é registrado, e ele expõe os repositórios.

---

## Checklist

- [ ] Criar `I[Entity]Repository.cs` em `Domain/Abstractions/Repositories/`
- [ ] Adicionar propriedade `I[Entity]Repository [Entities] { get; }` em `IUnitOfWork`
- [ ] Criar `[Entity]Repository.cs` em `Infrastructure/Persistence/Repositories/`
- [ ] Adicionar a propriedade correspondente em `UnitOfWork`
- [ ] Criar `[Entity]Map.cs` em `Infrastructure/Persistence/Mappings/`
- [ ] Gerar e aplicar migration: `dotnet ef migrations add Add[Entity] -p src/Board.Infrastructure -s src/Board.Api`
