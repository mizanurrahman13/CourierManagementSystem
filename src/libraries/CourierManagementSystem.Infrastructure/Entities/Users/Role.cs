using Microsoft.AspNetCore.Identity;

namespace CourierManagementSystem.Infrastructure.Entities.Users;

public class Role : IdentityRole<Guid>
{
    public Role()
        : base()
    {
    }

    public Role(string roleName)
        : base(roleName)
    {
    }
}