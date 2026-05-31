namespace Board.Application.DTOs;

public sealed record TokenResult(string Token, DateTimeOffset ExpiresAt);
