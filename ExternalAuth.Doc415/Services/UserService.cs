using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;

namespace ProductManagement.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserViewModel>> GetUsers()
    {
        var users = await (from u in _context.Users
                           join ur in _context.UserRoles on u.Id equals ur.UserId
                           join r in _context.Roles on ur.RoleId equals r.Id
                           select new
                           UserViewModel
                           {
                               Id = u.Id,
                               Email = u.Email,
                               Role = r.Name
                           })
               .ToListAsync();

        return users;
    }

    public async Task<ApplicationUser> GetUserById(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task DeleteUser(string Id)
    {
        var user = await GetUserById(Id);
        _context.Remove(user);
        _context.SaveChangesAsync();

    }


}