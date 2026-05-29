# Tratamento de Erros

Como funciona o tratamento de erros no projeto.

---

## Visão geral

Erros de negócio são comunicados via **exceções tipadas**. O `GlobalExceptionHandler` captura tudo e serializa a resposta no formato **ProblemDetails** (RFC 9457).

```
Handler → lança BoardException
            ↓
      GlobalExceptionHandler
            ↓
      ProblemDetails (JSON)
            ↓
      HTTP Response (4xx / 5xx)
```

O controller não precisa de try/catch — basta deixar a exceção propagar.

---

## Hierarquia de exceções

```
BoardException  (abstrata — HttpStatusCode obrigatório)
├── BadRequestException   → 400  (validação com dicionário de erros)
├── NotFoundException     → 404
├── ConflictException     → 409
└── ... novas exceções aqui
```

Todas vivem em `Board.Application/Exceptions/`.

### Criando uma nova exceção

```csharp
using System.Net;

namespace Board.Application.Exceptions;

public sealed class NotFoundException(string message)
    : BoardException(message, HttpStatusCode.NotFound);
```

Basta herdar de `BoardException` e passar o `HttpStatusCode` correto — o `GlobalExceptionHandler` já faz o resto.

---

## Usando nos handlers

```csharp
var board = await unitOfWork.Boards.GetByIdAsync(query.Id, cancellationToken);

if (board is null)
    throw new NotFoundException("Board não encontrado.");

// Se chegou aqui, tudo certo — retorna o valor diretamente
return new GetBoardByIdQueryResponse(board.Id, board.Name);
```

---

## Validação automática (BadRequestException)

O `ValidationBehavior` intercepta toda requisição antes do handler. Se o validator encontrar erros, lança `BadRequestException` com um dicionário agrupado por campo. O handler nunca executa.

Resposta gerada automaticamente:

```json
{
  "title": "Dados inválidos.",
  "status": 400,
  "instance": "/api/v1/boards",
  "errors": {
    "Name": ["O nome não pode estar vazio.", "O nome pode ter no máximo 100 caracteres."],
    "Description": ["A descrição pode ter no máximo 500 caracteres."]
  }
}
```

---

## GlobalExceptionHandler

Captura todas as exceções não tratadas e retorna `ProblemDetails`:

| Exceção | Status | Tipo de resposta |
|---------|--------|-----------------|
| `BadRequestException` | 400 | `HttpValidationProblemDetails` (com `errors`) |
| Qualquer `BoardException` | Código da exceção | `ProblemDetails` |
| Qualquer outra | 500 | `ProblemDetails` com mensagem genérica |

Detalhes internos nunca vazam para o cliente em erros 500.

---

## Formato de resposta

### Erro de negócio (ex: 404)

```json
{
  "title": "Board não encontrado.",
  "status": 404,
  "instance": "/api/v1/boards/abc123"
}
```

### Erro de validação (400)

```json
{
  "title": "Dados inválidos.",
  "status": 400,
  "instance": "/api/v1/boards",
  "errors": {
    "Name": ["O nome não pode estar vazio."]
  }
}
```

### Erro interno (500)

```json
{
  "title": "Ocorreu um erro inesperado. Tente novamente mais tarde.",
  "status": 500,
  "instance": "/api/v1/boards"
}
```

---

## Quando usar o quê

| Situação | O que fazer |
|----------|-------------|
| Recurso não encontrado | `throw new NotFoundException("...")` |
| Dado duplicado | `throw new ConflictException("...")` |
| Entrada inválida (campos) | FluentValidation no validator |
| Regra de negócio violada | `throw new BadRequestException(...)` |
| Erro inesperado | deixar propagar — o handler trata como 500 |
