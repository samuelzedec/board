# Board API

API REST para gerenciamento de boards, construída com ASP.NET Core 10, EF Core e PostgreSQL.

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)

---

## Opção 1 — Rodar localmente (API na máquina + banco no Docker)

### 1. Suba o banco de dados

```bash
docker compose up -d database
```

### 2. Configure a connection string via user secrets

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5432;Database=board;Username=postgres;Password=postgres" \
  --project src/Board.Api
```

### 3. Rode a API

```bash
dotnet run --project src/Board.Api
```

As migrações são aplicadas automaticamente na inicialização.

Acesse em: `http://localhost:5201/swagger`

---

## Opção 2 — Rodar tudo com Docker (API + banco)

```bash
docker compose up --build
```

Acesse em: `http://localhost:8080/swagger`

> O Swagger só fica disponível no ambiente `Development`.

---

## Migrações

Restaure as ferramentas locais antes de usar o `dotnet-ef`:

```bash
dotnet tool restore
```

### Criar uma nova migração

```bash
dotnet ef migrations add NomeDaMigracao \
  --project src/Board.Infrastructure \
  --startup-project src/Board.Api
```

### Aplicar manualmente

```bash
dotnet ef database update \
  --project src/Board.Infrastructure \
  --startup-project src/Board.Api
```

> As migrações também são aplicadas automaticamente ao iniciar a aplicação.

---

## Estrutura do projeto

```
src/
├── Board.Api            # Controllers, middlewares, configuração HTTP
├── Board.Application    # Features (CQRS), validações, exceções
├── Board.Infrastructure # EF Core, repositórios, migrations
└── Board.Domain         # Entidades, interfaces de repositório
```

Consulte a pasta `docs/` para detalhes sobre o modelo de dados.

---

## Health check

```
GET /
```

---

## Variáveis de ambiente (Docker)

| Variável | Descrição |
|----------|-----------|
| `ASPNETCORE_ENVIRONMENT` | `Development` ou `Production` |
| `ConnectionStrings__DefaultConnection` | String de conexão com o PostgreSQL |
