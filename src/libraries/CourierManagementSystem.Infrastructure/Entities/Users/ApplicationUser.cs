using Microsoft.AspNetCore.Identity;

namespace CourierManagementSystem.Infrastructure.Entities.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
