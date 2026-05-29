# Criando uma Feature (CQRS)

Como criar Commands, Queries e Controllers usando **Mediator** e **FluentValidation**.

---

## Estrutura de pastas

```
src/
├── Board.Application/
│   └── Features/
│       └── [Entidade]/
│           └── [Feature]/
│               ├── [Feature]Command.cs           (ou Query)
│               ├── [Feature]CommandHandler.cs    (ou QueryHandler)
│               ├── [Feature]CommandValidator.cs  (ou QueryValidator)
│               └── [Feature]CommandResponse.cs   (ou QueryResponse — se tiver retorno)
└── Board.Api/
    └── Controllers/
        └── [Entidade]Controller.cs
```

---

## Commands (operações de escrita)

### 1. Command

Record que carrega os dados da operação.

```csharp
// Features/Boards/CreateBoard/CreateBoardCommand.cs
using Mediator;

namespace Board.Application.Features.Boards.CreateBoard;

public sealed record CreateBoardCommand(string Name, string Description)
    : ICommand<CreateBoardCommandResponse>;
```

Use `ICommand<TResponse>` quando o handler precisa retornar dados (ex: o Id do recurso criado).  
Use `ICommand` quando não há retorno (ex: deletar, atualizar sem resposta).

### 2. Command Handler

```csharp
// Features/Boards/CreateBoard/CreateBoardCommandHandler.cs
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Boards.CreateBoard;

internal sealed class CreateBoardCommandHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateBoardCommand, CreateBoardCommandResponse>
{
    public async ValueTask<CreateBoardCommandResponse> Handle(
        CreateBoardCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.Boards.ExistsWithNameAsync(command.Name, cancellationToken);

        if (exists)
            throw new ConflictException("Já existe um board com esse nome.");

        var board = new Board(command.Name, command.Description);
        await unitOfWork.Boards.AddAsync(board, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBoardCommandResponse(board.Id);
    }
}
```

- Nunca retorna `null` — se der erro, lança uma exceção da hierarquia `BoardException`.
- Salvar sempre via `unitOfWork.SaveChangesAsync()`.

### 3. Command Validator

```csharp
// Features/Boards/CreateBoard/CreateBoardCommandValidator.cs
using FluentValidation;

namespace Board.Application.Features.Boards.CreateBoard;

internal sealed class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O nome não pode estar vazio.")
            .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres.");

        RuleFor(c => c.Description)
            .MaximumLength(500).WithMessage("A descrição pode ter no máximo 500 caracteres.");
    }
}
```

- O `ValidationBehavior` intercepta automaticamente toda requisição e dispara o validator antes do handler.
- Se houver erros de validação, o handler nunca chega a executar — o `GlobalExceptionHandler` responde com `400`.

### 4. Command Response

```csharp
// Features/Boards/CreateBoard/CreateBoardCommandResponse.cs
namespace Board.Application.Features.Boards.CreateBoard;

public sealed record CreateBoardCommandResponse(Guid Id);
```

---

## Queries (operações de leitura)

### 1. Query

```csharp
// Features/Boards/GetBoardById/GetBoardByIdQuery.cs
using Mediator;

namespace Board.Application.Features.Boards.GetBoardById;

public sealed record GetBoardByIdQuery(Guid Id) : IQuery<GetBoardByIdQueryResponse>;
```

### 2. Query Handler

```csharp
// Features/Boards/GetBoardById/GetBoardByIdQueryHandler.cs
using Board.Application.Exceptions;
using Board.Domain.Repositories;
using Mediator;

namespace Board.Application.Features.Boards.GetBoardById;

internal sealed class GetBoardByIdQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetBoardByIdQuery, GetBoardByIdQueryResponse>
{
    public async ValueTask<GetBoardByIdQueryResponse> Handle(
        GetBoardByIdQuery query,
        CancellationToken cancellationToken)
    {
        var board = await unitOfWork.Boards.GetByIdAsync(query.Id, cancellationToken);

        if (board is null)
            throw new NotFoundException("Board não encontrado.");

        return new GetBoardByIdQueryResponse(board.Id, board.Name, board.Description);
    }
}
```

- Queries leem dados — não chamam `SaveChangesAsync`.

### 3. Query Response

```csharp
// Features/Boards/GetBoardById/GetBoardByIdQueryResponse.cs
namespace Board.Application.Features.Boards.GetBoardById;

public sealed record GetBoardByIdQueryResponse(Guid Id, string Name, string Description);
```

---

## Controller

```csharp
// Controllers/BoardsController.cs
using Board.Application.Features.Boards.CreateBoard;
using Board.Application.Features.Boards.GetBoardById;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class BoardsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetBoardByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBoardCommand command,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
}
```

- Injete `ISender` (não `IMediator`) — é a interface mínima necessária para enviar mensagens.
- O controller só despacha — sem lógica de negócio.
- Use os métodos do `ControllerBase`: `Ok()`, `Created()`, `CreatedAtAction()`, `NoContent()`, `NotFound()`.
- Erros são tratados pelo `GlobalExceptionHandler` automaticamente — não precisa de try/catch.

---

## Hierarquia de exceções

Para sinalizar erros de negócio, lance exceções da hierarquia `BoardException`:

```csharp
// Exemplos de exceções disponíveis em Board.Application.Exceptions
throw new NotFoundException("Board não encontrado.");       // 404
throw new ConflictException("Nome já está em uso.");        // 409
throw new BadRequestException(errors);                      // 400 com detalhes
```

Novas exceções: crie uma classe em `Board.Application/Exceptions/` herdando de `BoardException` e passe o `HttpStatusCode` correspondente.

---

## Checklist para nova feature

- [ ] Criar pasta `Features/[Entidade]/[Feature]/`
- [ ] `[Feature]Command.cs` ou `[Feature]Query.cs` implementando `ICommand<T>` ou `IQuery<T>`
- [ ] `[Feature]CommandHandler.cs` ou `[Feature]QueryHandler.cs`
- [ ] `[Feature]CommandValidator.cs` com as regras de validação
- [ ] `[Feature]CommandResponse.cs` ou `[Feature]QueryResponse.cs` (se houver retorno)
- [ ] Criar ou atualizar o controller em `Controllers/`
