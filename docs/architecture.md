# Arquitetura

Visão geral da arquitetura do projeto Board API.

---

## Estrutura da solução

```
/
├── src/
│   ├── Board.Api            # Camada HTTP (controllers, middleware, setup)
│   ├── Board.Application    # Lógica de negócio (CQRS, validação, exceções)
│   ├── Board.Infrastructure # Persistência (EF Core, repositórios, migrations)
│   └── Board.Domain         # Entidades, repositórios (interfaces)
├── docs/
├── compose.yml
└── Dockerfile
```

---

## Camadas e dependências

```
Api → Application → Domain
 └─→ Infrastructure → Application
                   └→ Domain
```

- **Domain** não depende de nada.
- **Application** depende apenas de **Domain**.
- **Infrastructure** depende de **Application** e **Domain**.
- **Api** depende de **Infrastructure** (Application e Domain ficam disponíveis transitivamente).

---

## Fluxo de uma requisição

```
HTTP Request
  → Controller
  → ISender.Send(command/query)
  → ValidationBehavior (FluentValidation)
  → Handler (lógica de negócio)
  → UnitOfWork / Repository (EF Core)
  → Retorno direto ao controller
  → HTTP Response
```

Erros lançados como `BoardException` (ou subclasses) são capturados pelo `GlobalExceptionHandler`, que serializa a resposta no formato `ProblemDetails`.

---

## CQRS

O projeto usa CQRS com o pacote **Mediator** (source generator).

- **Commands** representam operações de escrita (`ICommand<TResponse>` ou `ICommand`).
- **Queries** representam operações de leitura (`IQuery<TResponse>`).
- Cada operação tem seu próprio handler, validator e response DTO.
- A `ValidationBehavior<TRequest, TResponse>` intercepta toda requisição antes do handler.

---

## Inicialização

### BuilderSetup

Configura os serviços no container de DI:

```csharp
extension(WebApplicationBuilder builder)
{
    public void Configure()
    {
        builder.ConfigureApiDocumentation();  // AddControllers, Swagger
        builder.ConfigureExceptionHandling(); // ProblemDetails, GlobalExceptionHandler
        builder.ConfigureLayers();            // Application + Infrastructure
    }
}
```

### PipelineSetup

Configura o pipeline HTTP:

```csharp
extension(WebApplication app)
{
    public async Task ConfigureAsync()
    {
        app.ConfigureHealthCheck();      // /health
        app.ConfigureApiDocumentation(); // Swagger (só em Development)
        app.UseExceptionHandler();
        app.MapControllers();
        await app.ApplyMigrationsAsync();
    }
}
```

---

## Persistência

- **ORM**: Entity Framework Core com Npgsql (PostgreSQL).
- **Migrações**: Aplicadas automaticamente na inicialização (`ApplyMigrationsAsync`).
- **Mapeamento**: Cada entidade tem seu `EntityTypeConfiguration` herdando de `BaseEntityTypeConfiguration<T>`.

---

## Observabilidade

- **Health check**: `/health` com resposta detalhada via `HealthChecks.UI.Client`.
- **API docs**: Swagger (`/swagger`) disponível apenas em Development.
- **Logging**: `ILogger<T>` injetado nos handlers onde necessário.
