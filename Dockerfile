FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /app

COPY Directory.Build.props Directory.Packages.props ./
COPY src/Board.Api/Board.Api.csproj ./src/Board.Api/
COPY src/Board.Application/Board.Application.csproj ./src/Board.Application/
COPY src/Board.Infrastructure/Board.Infrastructure.csproj ./src/Board.Infrastructure/
COPY src/Board.Domain/Board.Domain.csproj ./src/Board.Domain/

RUN dotnet restore ./src/Board.Api/Board.Api.csproj

COPY src/ ./src/
RUN dotnet build ./src/Board.Api/Board.Api.csproj \
    --no-restore \
    -c Release

FROM build AS publish
WORKDIR /app

RUN dotnet publish src/Board.Api/Board.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    --no-build

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

RUN addgroup -g 1000 appgroup && \
    adduser -u 1000 -G appgroup -s /bin/sh -D appuser && \
    apk add --no-cache bash

COPY --from=publish /app/publish .
RUN chown -R appuser:appgroup /app
USER appuser

EXPOSE 8080
ENTRYPOINT ["dotnet", "Board.Api.dll"]