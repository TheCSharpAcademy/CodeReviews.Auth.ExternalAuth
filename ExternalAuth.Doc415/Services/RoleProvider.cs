namespace ProductManagement.Services;

public class RoleProvider
{
    public string[] Roles { get; set; }

    public RoleProvider()
    {
        Roles = new string[2] { "Admin", "User" };
    }

    public string[] GetRoles() { return Roles; }

}
