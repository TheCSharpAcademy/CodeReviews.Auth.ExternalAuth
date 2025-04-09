using System.ComponentModel.DataAnnotations;

namespace ProductManagement.hasona23.Constants;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";
    public const string Staff = "Staff";
    
    public static string[] GetAllRoles()
    {
        return [Admin, Customer, Staff];
    }
    
}