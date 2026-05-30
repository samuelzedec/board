# Feature: Comment

Comentários permitem que usuários deixem observações em um card.
Cada comentário pertence a um card e tem um autor (o usuário autenticado que o criou).

> Todos os endpoints exigem autenticação.

---

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/cards/{cardId}/comments` | Cria um novo comentário |
| `PUT` | `/cards/{cardId}/comments/{commentId}` | Edita o conteúdo de um comentário |
| `DELETE` | `/cards/{cardId}/comments/{commentId}` | Remove um comentário |
| `GET` | `/cards/{cardId}/comments` | Lista todos os comentários do card |

---

## POST `/cards/{cardId}/comments`

Cria um novo comentário em um card. O autor é o próprio usuário autenticado.

**Request body**
```json
{
  "content": "Precisamos revisar os requisitos antes de continuar."
}
```

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `content` | string | sim | Texto do comentário (não pode ser vazio) |

**Response `201 Created`**
```json
{
  "id": "uuid",
  "cardId": "uuid",
  "authorId": "uuid",
  "content": "Precisamos revisar os requisitos antes de continuar.",
  "editedAt": null,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Regras**
- O card deve existir
- Qualquer usuário autenticado pode comentar, não apenas o dono do projeto
- `content` não pode ser vazio ou composto só de espaços

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `404 Not Found` | Card não encontrado |
| `422 Unprocessable Entity` | Dados inválidos (content vazio, etc.) |

---

## PUT `/cards/{cardId}/comments/{commentId}`

Edita o conteúdo de um comentário. Ao editar, o campo `editedAt` é preenchido automaticamente com a data/hora atual.

**Request body**
```json
{
  "content": "Revisando: precisamos alinhar com o cliente antes de continuar."
}
```

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `content` | string | sim | Novo texto do comentário (não pode ser vazio) |

**Response `200 OK`**
```json
{
  "id": "uuid",
  "cardId": "uuid",
  "authorId": "uuid",
  "content": "Revisando: precisamos alinhar com o cliente antes de continuar.",
  "editedAt": "2025-01-02T09:00:00Z",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Regras**
- O card e o comentário devem existir
- Apenas o autor do comentário pode editar
- `content` não pode ser vazio ou composto só de espaços
- `editedAt` é preenchido automaticamente, não deve ser enviado no body

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é o autor do comentário |
| `404 Not Found` | Card ou comentário não encontrados |
| `422 Unprocessable Entity` | Dados inválidos (content vazio, etc.) |

---

## DELETE `/cards/{cardId}/comments/{commentId}`

Remove um comentário.

**Response `204 No Content`**

**Regras**
- O card e o comentário devem existir
- Apenas o autor do comentário ou o dono do projeto pode deletar

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é o autor nem o dono do projeto |
| `404 Not Found` | Card ou comentário não encontrados |

---

## GET `/cards/{cardId}/comments`

Retorna todos os comentários de um card ordenados do mais recente para o mais antigo (`createdAt` decrescente).

**Response `200 OK`**
```json
[
  {
    "id": "uuid",
    "cardId": "uuid",
    "authorId": "uuid",
    "content": "Precisamos revisar os requisitos antes de continuar.",
    "editedAt": null,
    "createdAt": "2025-01-01T12:00:00Z"
  },
  {
    "id": "uuid",
    "cardId": "uuid",
    "authorId": "uuid",
    "content": "Concordo, vou verificar amanhã.",
    "editedAt": null,
    "createdAt": "2025-01-01T10:00:00Z"
  }
]
```

**Regras**
- O card deve existir
- Qualquer usuário autenticado pode ver os comentários

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `404 Not Found` | Card não encontrado |
