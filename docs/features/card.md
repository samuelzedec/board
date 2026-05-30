# Feature: Card

Cards representam as tarefas dentro de uma coluna. Cada card pertence a uma coluna,
pode ter um responsável, uma prioridade, uma data de vencimento e uma descrição.

> Todos os endpoints exigem autenticação.

---

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/columns/{columnId}/cards` | Cria um novo card |
| `DELETE` | `/columns/{columnId}/cards/{cardId}` | Remove um card |
| `GET` | `/columns/{columnId}/cards` | Lista todos os cards da coluna |
| `GET` | `/columns/{columnId}/cards/{cardId}` | Retorna um card pelo ID |

---

## POST `/columns/{columnId}/cards`

Cria um novo card dentro de uma coluna.

**Request body**
```json
{
  "title": "Implementar tela de login",
  "description": "Criar a tela de login com validação de campos",
  "order": 1,
  "priority": "High",
  "dueDate": "2025-03-01",
  "assigneeId": "uuid"
}
```

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `title` | string | sim | Título do card (máx. 255 caracteres) |
| `description` | string | não | Descrição detalhada |
| `order` | number | sim | Posição do card dentro da coluna (começa em 1) |
| `priority` | string | não | `None`, `Low`, `Medium`, `High` ou `Urgent`. Padrão: `None` |
| `dueDate` | string (date) | não | Data de vencimento no formato `YYYY-MM-DD` |
| `assigneeId` | uuid | não | ID do usuário responsável pelo card |

**Response `201 Created`**
```json
{
  "id": "uuid",
  "columnId": "uuid",
  "title": "Implementar tela de login",
  "description": "Criar a tela de login com validação de campos",
  "order": 1,
  "priority": "High",
  "dueDate": "2025-03-01",
  "assigneeId": "uuid",
  "updatedAt": "2025-01-01T00:00:00Z",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Regras**
- A coluna deve existir
- O usuário autenticado deve ser o dono do projeto ao qual a coluna pertence
- Não pode existir outro card com o mesmo `order` na mesma coluna
- `title` não pode ser vazio
- Se informado, o `assigneeId` deve ser de um usuário existente

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Coluna não encontrada ou `assigneeId` não corresponde a nenhum usuário |
| `409 Conflict` | Já existe um card com essa ordem nessa coluna |
| `422 Unprocessable Entity` | Dados inválidos (campo obrigatório ausente, `priority` inválido, etc.) |

---

## DELETE `/columns/{columnId}/cards/{cardId}`

Remove um card e todos os comentários dentro dele.

**Response `204 No Content`**

**Regras**
- A coluna e o card devem existir
- O usuário autenticado deve ser o dono do projeto ao qual a coluna pertence
- Ao deletar o card, todos os comentários associados também são removidos (cascade)

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Coluna ou card não encontrados |

---

## GET `/columns/{columnId}/cards`

Retorna todos os cards da coluna ordenados por `order`.

**Response `200 OK`**
```json
[
  {
    "id": "uuid",
    "columnId": "uuid",
    "title": "Implementar tela de login",
    "description": "Criar a tela de login com validação de campos",
    "order": 1,
    "priority": "High",
    "dueDate": "2025-03-01",
    "assigneeId": "uuid",
    "updatedAt": "2025-01-01T00:00:00Z",
    "createdAt": "2025-01-01T00:00:00Z"
  }
]
```

**Regras**
- A coluna deve existir
- O usuário autenticado deve ser o dono do projeto ao qual a coluna pertence

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Coluna não encontrada |

---

## GET `/columns/{columnId}/cards/{cardId}`

Retorna um card específico pelo ID.

**Response `200 OK`**
```json
{
  "id": "uuid",
  "columnId": "uuid",
  "title": "Implementar tela de login",
  "description": "Criar a tela de login com validação de campos",
  "order": 1,
  "priority": "High",
  "dueDate": "2025-03-01",
  "assigneeId": "uuid",
  "updatedAt": "2025-01-01T00:00:00Z",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é dono do projeto |
| `404 Not Found` | Coluna ou card não encontrados |
