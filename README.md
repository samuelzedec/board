<div align="center">

# 🗂️ Board API

**API REST para gerenciamento de quadros de tarefas (estilo Kanban)**, construída com arquitetura em camadas, CQRS e autenticação JWT.

<br/>

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

</div>

---

## 📑 Índice

- [Sobre o projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Stack & Tecnologias](#-stack--tecnologias)
- [Arquitetura](#-arquitetura)
- [Pré-requisitos](#-pré-requisitos)
- [Como executar](#-como-executar)
- [Migrações](#-migrações)
- [Endpoints](#-endpoints)
- [Estrutura do projeto](#-estrutura-do-projeto)
- [Variáveis de ambiente](#-variáveis-de-ambiente)
- [Documentação](#-documentação)

---

## 🎯 Sobre o projeto

A **Board API** permite organizar o trabalho em **projetos**, divididos em **colunas**, que contêm **cards** (tarefas) e seus **comentários**. O acesso é controlado por autenticação **JWT**, e a persistência é feita em **PostgreSQL** via **Entity Framework Core**.

Projeto desenvolvido como trabalho final do módulo de **Desenvolvimento em C#**, aplicando arquitetura em camadas, DTOs, services/handlers, repositórios e tratamento centralizado de erros.

---

## ✨ Funcionalidades

- 🔐 **Autenticação e autorização** com JWT (registro, login e endpoint protegido `me`).
- 👤 **Usuários** com senha protegida por hash (BCrypt).
- 💬 **Comentários** em cards com CRUD completo (criar, listar, editar e remover), restritos ao autor.
- 🧱 **Modelo de domínio** com 5 entidades relacionadas: `User`, `Project`, `Column`, `Card` e `Comment`.
- 📋 **Validação automática** de entrada com FluentValidation.
- 🧯 **Tratamento global de erros** padronizado em `ProblemDetails` (RFC 9457).
- 📖 **Documentação interativa** via Swagger/OpenAPI.
- ❤️ **Health check** e migrações aplicadas automaticamente na inicialização.

---

## 🧰 Stack & Tecnologias

| Categoria | Tecnologia |
|-----------|-----------|
| Linguagem | C# (.NET 10) |
| Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core + Npgsql |
| Banco de dados | PostgreSQL |
| Autenticação | JWT Bearer |
| Hash de senha | BCrypt |
| CQRS / Mediator | [Mediator](https://github.com/martinothamar/Mediator) (source generator) |
| Validação | FluentValidation |
| Documentação | Swagger / OpenAPI |
| Containerização | Docker / Docker Compose |

---

## 🏛️ Arquitetura

Arquitetura **em camadas** com CQRS, separando entrada HTTP, regras de negócio, domínio e infraestrutura:

```
Cliente / Swagger
        ↓
   Board.Api            → Controllers, middlewares, autenticação, Swagger
        ↓
   Board.Application    → CQRS (Commands/Queries/Handlers), DTOs, validação, exceções
        ↓
   Board.Domain         → Entidades, enums e interfaces de repositório
        ↓
   Board.Infrastructure → EF Core, DbContext, repositórios, migrations
        ↓
     PostgreSQL
```

> Detalhes completos em [`docs/architecture.md`](docs/architecture.md).

---

## ✅ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)

---

## 🚀 Como executar

### Opção 1 — API local + banco no Docker

```bash
# 1. Suba o banco de dados
docker compose up -d database

# 2. Configure a connection string via user secrets
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5432;Database=board;Username=postgres;Password=postgres" \
  --project src/Board.Api

# 3. Rode a API
dotnet run --project src/Board.Api
```

As migrações são aplicadas automaticamente na inicialização.
Acesse o Swagger em: **http://localhost:5201/swagger**

### Opção 2 — Tudo com Docker (API + banco)

```bash
docker compose up --build
```

Acesse o Swagger em: **http://localhost:8080/swagger**

> O Swagger só fica disponível no ambiente `Development`.

---

## 🗄️ Migrações

Restaure as ferramentas locais antes de usar o `dotnet-ef`:

```bash
dotnet tool restore
```

**Criar uma nova migração:**

```bash
dotnet ef migrations add NomeDaMigracao \
  --project src/Board.Infrastructure \
  --startup-project src/Board.Api
```

**Aplicar manualmente:**

```bash
dotnet ef database update \
  --project src/Board.Infrastructure \
  --startup-project src/Board.Api
```

> As migrações também são aplicadas automaticamente ao iniciar a aplicação.

---

## 🔌 Endpoints

> 🔒 = requer autenticação (token JWT no header `Authorization: Bearer <token>`).

| Método | Rota | Descrição | Auth |
|--------|------|-----------|:----:|
| `POST` | `/api/users` | Registra um novo usuário | — |
| `POST` | `/api/auth/login` | Autentica e retorna um token JWT | — |
| `GET` | `/api/auth/me` | Dados do usuário autenticado | 🔒 |
| `GET` | `/api/users` | Lista usuários | 🔒 |
| `GET` | `/api/users/{id}` | Busca usuário por id | 🔒 |
| `POST` | `/api/cards/{cardId}/comments` | Cria um comentário no card | 🔒 |
| `GET` | `/api/cards/{cardId}/comments` | Lista os comentários do card | 🔒 |
| `PUT` | `/api/cards/{cardId}/comments/{commentId}` | Edita um comentário (apenas o autor) | 🔒 |
| `DELETE` | `/api/cards/{cardId}/comments/{commentId}` | Remove um comentário (apenas o autor) | 🔒 |
| `GET` | `/` | Health check | — |

> Os endpoints de **Projects**, **Columns** e **Cards** estão em desenvolvimento. Consulte a pasta [`docs/features/`](docs/features) para os contratos detalhados de cada recurso.

---

## 📂 Estrutura do projeto

```
src/
├── Board.Api            # Controllers, middlewares, configuração HTTP
├── Board.Application    # Features (CQRS), validações, exceções
├── Board.Infrastructure # EF Core, repositórios, migrations
└── Board.Domain         # Entidades, interfaces de repositório
docs/                    # Documentação de arquitetura, features e padrões
```

---

## 🔧 Variáveis de ambiente

| Variável | Descrição |
|----------|-----------|
| `ASPNETCORE_ENVIRONMENT` | `Development` ou `Production` |
| `ConnectionStrings__DefaultConnection` | String de conexão com o PostgreSQL |

---

## 📚 Documentação

| Documento | Conteúdo |
|-----------|----------|
| [`docs/architecture.md`](docs/architecture.md) | Arquitetura, camadas e fluxo de requisição |
| [`docs/style-guide.md`](docs/style-guide.md) | Convenções de código |
| [`docs/error-handling.md`](docs/error-handling.md) | Tratamento de erros e exceções |
| [`docs/new-repository.md`](docs/new-repository.md) | Como adicionar um novo repositório |
| [`docs/features/`](docs/features) | Contratos detalhados de cada feature |