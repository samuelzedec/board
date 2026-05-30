# Feature: Project

Projetos são o nível mais alto do board. Cada projeto pertence a um usuário (o dono)
e contém as colunas que organizam os cards.

> Todos os endpoints exigem autenticação.

---

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/projects` | Cria um novo projeto |
| `DELETE` | `/projects/{projectId}` | Remove um projeto |
| `GET` | `/projects` | Lista todos os projetos do usuário autenticado |
| `GET` | `/projects/{projectId}` | Retorna um projeto pelo ID |

---

## POST `/projects`

Cria um novo projeto. O dono é o próprio usuário autenticado.

**Request body**
```json
{
  "name": "Board Pessoal",
  "description": "Projeto para organizar minhas tarefas pessoais",
  "color": "#4A90E2"
}
```

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `name` | string | sim | Nome do projeto (máx. 100 caracteres) |
| `description` | string | não | Descrição do projeto |
| `color` | string | sim | Cor de identificação do projeto (máx. 20 caracteres) |

**Response `201 Created`**
```json
{
  "id": "uuid",
  "ownerId": "uuid",
  "name": "Board Pessoal",
  "description": "Projeto para organizar minhas tarefas pessoais",
  "color": "#4A90E2",
  "isActive": true,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Regras**
- `name` não pode ser vazio
- `color` não pode ser vazio

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `422 Unprocessable Entity` | Dados inválidos (campo obrigatório ausente, valor muito longo, etc.) |

---

## DELETE `/projects/{projectId}`

Remove um projeto e tudo que está dentro dele (colunas, cards e comentários).

**Response `204 No Content`**

**Regras**
- O projeto deve existir
- Apenas o dono do projeto pode deletar
- Ao deletar o projeto, todas as colunas, cards e comentários associados também são removidos (cascade)

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é o dono do projeto |
| `404 Not Found` | Projeto não encontrado |

---

## GET `/projects`

Retorna todos os projetos do usuário autenticado ordenados por `createdAt` decrescente.

**Response `200 OK`**
```json
[
  {
    "id": "uuid",
    "ownerId": "uuid",
    "name": "Board Pessoal",
    "description": "Projeto para organizar minhas tarefas pessoais",
    "color": "#4A90E2",
    "isActive": true,
    "createdAt": "2025-01-01T00:00:00Z"
  }
]
```

**Regras**
- Retorna apenas os projetos que pertencem ao usuário autenticado

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |

---

## GET `/projects/{projectId}`

Retorna um projeto específico pelo ID.

**Response `200 OK`**
```json
{
  "id": "uuid",
  "ownerId": "uuid",
  "name": "Board Pessoal",
  "description": "Projeto para organizar minhas tarefas pessoais",
  "color": "#4A90E2",
  "isActive": true,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Erros possíveis**

| Status | Motivo |
|--------|--------|
| `401 Unauthorized` | Token ausente ou inválido |
| `403 Forbidden` | Usuário não é o dono do projeto |
| `404 Not Found` | Projeto não encontrado |
