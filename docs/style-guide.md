# Style Guide

Convenções de código C# adotadas neste projeto.

---

## Nomenclatura

### Arquivos e pastas

| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Endpoint | `[Feature]Endpoint.cs` | `CreateUserEndpoint.cs` |
| Grupo de endpoints | `[Entity]GroupEndpoint.cs` | `UserGroupEndpoint.cs` |
| Command | `[Feature]Command.cs` | `CreateUserCommand.cs` |
| Command handler | `[Feature]CommandHandler.cs` | `CreateUserCommandHandler.cs` |
| Command validator | `[Feature]CommandValidator.cs` | `CreateUserCommandValidator.cs` |
| Command response | `[Feature]CommandResponse.cs` | `CreateUserCommandResponse.cs` |
| Query | `[Feature]Query.cs` | `GetUserByIdQuery.cs` |
| Query handler | `[Feature]QueryHandler.cs` | `GetUserByIdQueryHandler.cs` |
| Query validator | `[Feature]QueryValidator.cs` | `GetUserByIdQueryValidator.cs` |
| Query response | `[Feature]QueryResponse.cs` | `GetUserByIdQueryResponse.cs` |
| Repositório | `[Entity]Repository.cs` | `UserRepository.cs` |
| Mapeamento EF | `[Entity]Map.cs` | `UserMap.cs` |
| Entidade | `[Entity].cs` | `User.cs` |
| Value object | `[Name].cs` | `Email.cs`, `Phone.cs` |
| Opções | `[Section]Options.cs` | `AgentOptions.cs` |

### Pastas por camada

```
Endpoints/[Entity]/
    [Entity]GroupEndpoint.cs
    [Feature]/
        [Feature]Endpoint.cs

Features/[Entity]/
    [Feature]/
        [Feature]Command.cs
        [Feature]CommandHandler.cs
        [Feature]CommandValidator.cs
        [Feature]CommandResponse.cs

Persistence/
    Mappings/
        [Entity]Map.cs
    Repositories/
        [Entity]Repository.cs
```

### Classes e membros

- **Classes abstratas** → nome descritivo sem prefixo `Abstract` → `BaseEntityTypeConfiguration<T>`
- **Implementações internas** → sempre `internal sealed class`
- **Registros (records)** → sempre `sealed record` para Commands, Queries e Responses
- **Interfaces** → prefixo `I` → `IUserRepository`, `IUnitOfWork`
- **Métodos async** → sufixo `Async` → `GetByIdAsync`, `SaveChangesAsync`
- **Métodos de fábrica em domínio** → `Create(...)` estático e privado no construtor
- **Métodos de conversão em response** → `MapFrom(entity)` estático e interno

---

## Namespaces

Seguem a estrutura de pastas. Nunca adicionar subnível desnecessário.

```csharp
Board.Api.Endpoints.Users.CreateUser
Board.Application.Features.Users.CreateUser
Board.Infrastructure.Persistence.Repositories
Board.Domain.Entities
Board.Domain.ValueObjects
```

---

## Modificadores de acesso

- Tudo que pode ser `internal` **deve ser** `internal`.
- Entidades e value objects de domínio são `public` (consumidos por outras camadas).
- Contratos CQRS (`ICommand`, `IQuery`, suas implementações record) são `public`.
- Handlers, validators e mappings são `internal sealed`.

---

## Estrutura de classes

### Ordem dos membros

1. Campos constantes / estáticos
2. Campos privados
3. Construtores (privado primeiro, depois público/interno)
4. Propriedades
5. Métodos públicos / internos
6. Métodos privados

### Construtores em entidades de domínio

```csharp
// Construtor vazio privado para EF Core
private User() { }

// Construtor de criação privado
private User(Guid externalId, Email email, Phone? phone) { ... }

// Método de fábrica público
public static User Create(Guid externalId, Email email, Phone? phone)
    => new(externalId, email, phone);
```

---

## Estilos de código

- **Expression-bodied members** sempre que o corpo for uma única linha.
- **Pattern matching** preferido a casts explícitos e `is`/`as`.
- **switch expressions** preferidos a `switch` statements.
- **Primary constructors** em handlers, middlewares e repositórios.
- **Collection expressions** `[item1, item2]` em vez de `new List<T> { }`.
- **`var`** apenas quando o tipo é óbvio pelo lado direito.
- Sem `this.` desnecessário.
- Sem comentários `// ...` para código removido — use git para isso.

---

## Strings e mensagens

- Todas as mensagens de erro e validação em **português do Brasil**.
- Mensagens de log em **inglês** (para ferramentas de observabilidade).
- Não concatenar strings com `+`; usar interpolação `$""` ou `string.Format`.

---

## Async / await

- Retornar `ValueTask` em handlers CQRS.
- Retornar `Task` em repositórios e serviços de infraestrutura.
- Sempre passar `CancellationToken` como último parâmetro com `default`.
- Nunca usar `.Result` ou `.Wait()` — sempre `await`.

---

## Configuração centralizada de pacotes

Versões de pacotes NuGet ficam **somente** em `Directory.Packages.props`.
Projetos referenciam o pacote sem a versão:

```xml
<!-- Directory.Packages.props -->
<PackageVersion Include="FluentValidation" Version="12.1.1" />

<!-- Board.Application.csproj -->
<PackageReference Include="FluentValidation" />
```

---

## EditorConfig

Configurações de formatação (tabs, encoding, trailing newline etc.) estão em `.editorconfig` na raiz. Não duplicar no projeto.
