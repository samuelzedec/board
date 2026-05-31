using System.ComponentModel.DataAnnotations;

namespace Board.Infrastructure.Options;

public sealed record TokenSettingsOptions
{
    [Required] public string SecretKey { get; init; } = string.Empty;
    [Required] public string Issuer { get; init; } = string.Empty;
    [Required] public string Audience { get; init; } = string.Empty;
    [Required] public int ExpirationInMinutes { get; init; }
}