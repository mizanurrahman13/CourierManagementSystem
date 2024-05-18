namespace CourierManagementSystem.Infrastructure.Services;

public interface ICurrentUserService
{
    Task<string> GetUsername();
    string GetUsernames();
}
