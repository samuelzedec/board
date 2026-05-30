# Feature: User

---

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/users` | Cria um novo usuário |
| `GET` | `/users` | Lista todos os usuários |

---

## POST `/users`

Cria um novo usuário. Não exige autenticação.

**Request body**
```json
{
  "name": "João Silva",
  "email": "joao@email.com",
  "password": "senha123"
}
```

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `name` | string | sim | Nome do usuário (máx. 100 caracteres) |
| `email` | string | sim | E-mail único (máx. 255 caracteres) |
| `password` | string | sim | Senha do usuário |

**Response `201 Created`**
```json
{
  "id": "uuid",
  "name": "João Silva",
  "email": "joao@email.com",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Regras**
- `email` deve ser único — não pode já existir no sistema
- A senha deve ser armazenada como hash, nunca em texto puro
- `passwordHash` nunca deve ser retornado na resposta

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `409 Conflict` | E-mail já cadastrado |
| `422 Unprocessable Entity` | Dados inválidos (campo obrigatório ausente, e-mail mal formatado, etc.) |

---

## GET `/users`

Lista todos os usuários cadastrados. Exige autenticação.

**Response `200 OK`**
```json
[
  {
    "id": "uuid",
    "name": "João Silva",
    "email": "joao@email.com",
    "createdAt": "2025-01-01T00:00:00Z"
  }
]
```

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
