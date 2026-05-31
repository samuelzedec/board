using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Board.Application.Abstractions;
using Board.Application.DTOs;
using Board.Domain.Entities;
using Board.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Board.Infrastructure.Security;

internal sealed class JwtTokenService(IOptions<TokenSettingsOptions> tokenSettings)
    : ITokenService
{
    public TokenResult GenerateToken(User user)
    {
        var settings = tokenSettings.Value;
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(settings.ExpirationInMinutes);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt.UtcDateTime,
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        return new TokenResult(token, expiresAt);
    }
}
