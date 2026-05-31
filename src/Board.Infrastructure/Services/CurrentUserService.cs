using System.Security.Claims;
using Board.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Board.Infrastructure.Services;

internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    private ClaimsPrincipal CurrentUser
        => httpContextAccessor.HttpContext?.User!;

    public Guid GetUserId()
    {
        var claim = CurrentUser.FindFirst(ClaimTypes.NameIdentifier)!;
        return Guid.Parse(claim.Value);
    }

    public string GetEmail()
    {
        var claim = CurrentUser.FindFirst(ClaimTypes.Email)!;
        return claim.Value;
    }
}
