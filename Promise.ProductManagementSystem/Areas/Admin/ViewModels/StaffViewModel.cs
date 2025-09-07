using Microsoft.AspNetCore.Identity;

namespace Promise.ProductManagementSystem.Areas.Admin.ViewModels
{
    public class StaffViewModel
    {
        public List<IdentityUser> StaffUsers { get; set; } = new List<IdentityUser>();
    }
}
