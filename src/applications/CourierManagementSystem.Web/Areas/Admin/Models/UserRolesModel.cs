namespace CourierManagementSystem.Web.Areas.Admin.Models;

public class UserRolesModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
