# Feature: Column

Colunas organizam os cards dentro de um projeto (ex: "To Do", "In Progress", "Done").
Cada coluna pertence a um projeto e possui uma ordem que define sua posição no board.

> Todos os endpoints exigem autenticação.

---

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/projects/{projectId}/columns` | Cria uma nova coluna |
| `DELETE` | `/projects/{projectId}/columns/{columnId}` | Remove uma coluna |
| `GET` | `/projects/{projectId}/columns` | Lista todas as colunas do projeto |
| `GET` | `/projects/{projectId}/columns/{columnId}` | Retorna uma coluna pelo ID |

---

## POST `/projects/{projectId}/columns`

Cria uma nova coluna dentro de um projeto.

**Request body**
```json
{
  "name": "In Progress",
  "order": 2
}
```

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `name` | string | sim | Nome da coluna (máx. 100 caracteres) |
| `order` | number | sim | Posição da coluna no board (começa em 1) |

**Response `201 Created`**
```json
{
  "id": "uuid",
  "projectId": "uuid",
  "name": "In Progress",
  "order": 2,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Regras**
- O projeto deve existir
- O usuário autenticado deve ser o dono do projeto
- Não pode existir outra coluna com o mesmo `order` no mesmo projeto
- `name` não pode ser vazio

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Projeto não encontrado |
| `409 Conflict` | Já existe uma coluna com essa ordem nesse projeto |
| `422 Unprocessable Entity` | Dados inválidos (campo obrigatório ausente, valor muito longo, etc.) |

---

## DELETE `/projects/{projectId}/columns/{columnId}`

Remove uma coluna e todos os cards dentro dela.

**Response `204 No Content`**

**Regras**
- O projeto e a coluna devem existir
- O usuário autenticado deve ser o dono do projeto
- Ao deletar a coluna, todos os cards associados também são removidos (cascade)

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Projeto ou coluna não encontrados |

---

## GET `/projects/{projectId}/columns`

Retorna todas as colunas do projeto ordenadas por `order`.

**Response `200 OK`**
```json
[
  {
    "id": "uuid",
    "projectId": "uuid",
    "name": "To Do",
    "order": 1,
    "createdAt": "2025-01-01T00:00:00Z"
  },
  {
    "id": "uuid",
    "projectId": "uuid",
    "name": "In Progress",
    "order": 2,
    "createdAt": "2025-01-01T00:00:00Z"
  }
]
```

**Regras**
- O projeto deve existir
- O usuário autenticado deve ser o dono do projeto

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Projeto não encontrado |

---

## GET `/projects/{projectId}/columns/{columnId}`

Retorna uma coluna específica pelo ID.

**Response `200 OK`**
```json
{
  "id": "uuid",
  "projectId": "uuid",
  "name": "To Do",
  "order": 1,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Projeto ou coluna não encontrados |
