# Board de Tarefas — Modelo de Dados

## Diagrama ER

```mermaid
erDiagram
    USER {
        guid Id PK
        string Name
        string Email
        string PasswordHash
        bool IsActive
        datetime CreatedAt
        datetime LastAccessAt
    }

    PROJECT {
        guid Id PK
        guid OwnerId FK
        string Name
        string Description
        string Color
        bool IsActive
        datetime CreatedAt
    }

    COLUMN {
        guid Id PK
        guid ProjectId FK
        string Name
        int Order
        datetime CreatedAt
    }

    CARD {
        guid Id PK
        guid ColumnId FK
        guid AssigneeId FK
        string Title
        string Description
        int Order
        date DueDate
        string Priority "Low|Medium|High|Urgent"
        datetime CreatedAt
        datetime UpdatedAt
    }

    COMMENT {
        guid Id PK
        guid CardId FK
        guid AuthorId FK
        string Content
        datetime CreatedAt
        datetime EditedAt
    }

    USER ||--o{ PROJECT : "owns"
    USER ||--o{ CARD : "assigned to"
    USER ||--o{ COMMENT : "writes"
    PROJECT ||--o{ COLUMN : "contains"
    COLUMN ||--o{ CARD : "groups"
    CARD ||--o{ COMMENT : "has"
```

---

## Relacionamentos

| Relation | Type | Description |
|---|---|---|
| User → Project | 1:N | A user can own multiple projects |
| User → Card | 1:N | A user can be assigned to multiple cards |
| User → Comment | 1:N | A user can write multiple comments |
| Project → Column | 1:N | A project has multiple columns (e.g. To Do, Doing, Done) |
| Column → Card | 1:N | A column groups multiple cards |
| Card → Comment | 1:N | A card can have multiple comments |

---

## Fluxo Principal

```mermaid
graph LR
    A[User logs in] --> B[Access Projects]
    B --> C[Select Project]
    C --> D[View Board]
    D --> E[Columns with Cards]
    E --> F[Create/Move/Edit Card]
    F --> G[Assign Responsible and Priority]
    G --> H[Add Comments to Card]
```
