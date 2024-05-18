using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CourierManagementSystem.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<string> GetUsername()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var currentUsername = !string.IsNullOrEmpty(userName)
            ? userName

            : "Anonymous";

        return currentUsername;
    }

    public string GetUsernames()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var currentUsername = !string.IsNullOrEmpty(userName)
            ? userName

            : "Anonymous";

        return currentUsername;
    }
}