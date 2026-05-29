# Board API

API REST para gerenciamento de boards, construída com ASP.NET Core 10, EF Core e PostgreSQL.

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop) (para o banco de dados)

---

## Configuração do ambiente

### 1. Suba o banco de dados

```bash
docker run -d \
  --name board-db \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=board \
  -p 5432:5432 \
  postgres:18-alpine
```

### 2. Configure a connection string

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5432;Database=board;Username=postgres;Password=postgres" \
  --project src/Board.Api
```

---

## Rodando localmente

```bash
dotnet run --project src/Board.Api
```

As migrações são aplicadas automaticamente na inicialização.

Acesse a documentação da API em: `http://localhost:5000/swagger`

---

## Rodando com Docker (API + banco juntos)

```bash
docker compose up --build
```

A API fica disponível em `http://localhost:8080/swagger`.

---

## Migrações

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

Consulte a pasta `docs/` para detalhes sobre arquitetura, features e tratamento de erros.

---

## Health check

```
GET /health
```

---

## Variáveis de ambiente (Docker)

| Variável | Descrição |
|----------|-----------|
| `ASPNETCORE_ENVIRONMENT` | `Development` ou `Production` |
| `ConnectionStrings__DefaultConnection` | String de conexão com o PostgreSQL |
