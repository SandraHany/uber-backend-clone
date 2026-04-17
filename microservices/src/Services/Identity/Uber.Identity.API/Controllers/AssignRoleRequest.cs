namespace Uber.Identity.API;

public class AssignRoleRequest
{
    public string Username { get; set; }
    public Role Role { get; set; }
}
