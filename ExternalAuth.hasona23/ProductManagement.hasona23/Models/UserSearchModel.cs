using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserSearchModel
{
    public IEnumerable<IdentityUser>? Users { get; set; }
    public string? SearchRole { get; set; }
    public string? SearchEmail { get; set; }
    public string? SearchUserName { get; set; }
    public bool? IsEmailConfirmed { get; set; }
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    public async Task<IEnumerable<IdentityUser>> SearchUsers(UserManager<IdentityUser> userManager)
    {
        var users = await userManager.Users.ToListAsync();

        if (!string.IsNullOrEmpty(SearchRole))
        {
            var roleUsers = new List<IdentityUser>();
            foreach (var user in users)
            {
                if (await userManager.IsInRoleAsync(user, SearchRole))
                {
                    roleUsers.Add(user);
                }
            }
            users = roleUsers;
        }

        if (IsEmailConfirmed.HasValue)
        {
            users = users.Where(user => user.EmailConfirmed == IsEmailConfirmed.Value).ToList();
        }

        if (!string.IsNullOrEmpty(SearchUserName))
        {
            users = users.Where(user => user.UserName != null &&
                                        user.UserName.Contains(SearchUserName)).ToList();
        }

        if (!string.IsNullOrEmpty(SearchEmail))
        {
            users = users.Where(user => user.Email != null &&
                                        user.Email.Contains(SearchEmail)).ToList();
        }

        return users;
    }
}