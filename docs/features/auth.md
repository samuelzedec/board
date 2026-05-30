# Feature: Auth

---

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/auth/login` | Autentica o usuário e retorna o token |
| `GET` | `/auth/me` | Retorna os dados do usuário autenticado |

---

## POST `/auth/login`

Não exige autenticação.

**Request body**
```json
{
  "email": "joao@email.com",
  "password": "senha123"
}
```

**Response `200 OK`**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresAt": "2025-01-02T00:00:00Z"
}
```

**Regras**
- Deve retornar `401` tanto para e-mail não encontrado quanto para senha errada — nunca indicar qual dos dois está errado

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Credenciais inválidas |
| `422 Unprocessable Entity` | Dados inválidos (campo obrigatório ausente, e-mail mal formatado, etc.) |

---

## GET `/auth/me`

Retorna os dados do usuário autenticado. Exige autenticação.

**Response `200 OK`**
```json
{
  "id": "uuid",
  "name": "Samuel Zedec",
  "email": "joao@email.com",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
