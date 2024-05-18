using CourierManagementSystem.Infrastructure.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace CourierManagementSystem.Infrastructure;
public static class ContextSeed
{
    public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Operator.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
    }
}
