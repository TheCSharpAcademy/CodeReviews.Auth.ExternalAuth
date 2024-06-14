using Microsoft.AspNetCore.Identity;
using ProductManagement.Models;

namespace ProductManagement.Data;

public class Seeder
{
    public IUserEmailStore<ApplicationUser> _userEmailStore { get; set; }
    public ILogger<Seeder> _logger { get; set; }
    public ApplicationDbContext _context { get; set; }
    public UserManager<ApplicationUser> _userManager;
    public IUserStore<ApplicationUser> _userStore;
    public Seeder(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<Seeder> logger, IUserStore<ApplicationUser> userStore)
    {
        _context = context;
        _userManager = userManager;
        _userStore = userStore;
        _logger = logger;
        _userEmailStore = (IUserEmailStore<ApplicationUser>)_userStore;
    }

    public async Task SeedAdmin()
    {
        try
        {
            var AdminUser = new ApplicationUser
            {
                UserName = "admin@product.com",
                Email = "admin@product.com",
                EmailConfirmed = true,
            };
            await _userStore.SetUserNameAsync(AdminUser, "admin@product.com", CancellationToken.None);

            await _userEmailStore.SetEmailAsync(AdminUser, "admin@product.com", CancellationToken.None);
            var result = await _userManager.CreateAsync(AdminUser, "Admin1234.");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error: {error.Code}, Description: {error.Description}");
                }
                return;
            }
            AdminUser = await _userManager.FindByEmailAsync(AdminUser.Email);
            await _userManager.AddToRoleAsync(AdminUser, "admin");

            _logger.LogInformation("Seeder created a new admin account with password.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            Console.WriteLine(ex.Message);
        }

    }

    public async Task SeedUser()
    {
        try
        {
            var User = new ApplicationUser
            {
                UserName = "user@product.com",
                Email = "user@product.com",
                EmailConfirmed = true,
            };
            await _userStore.SetUserNameAsync(User, "user@product.com", CancellationToken.None);

            await _userEmailStore.SetEmailAsync(User, "user@product.com", CancellationToken.None);
            var result = await _userManager.CreateAsync(User, "User1234.");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error: {error.Code}, Description: {error.Description}");
                }
                return;
            }
            User = await _userManager.FindByEmailAsync(User.Email);
            await _userManager.AddToRoleAsync(User, "user");

            _logger.LogInformation("Seeder created a new admin account with password.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            Console.WriteLine(ex.Message);
        }
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }

    private async Task SeedProducts()
    {
        string[] ProductName = new string[] { "Macaroni", "Olive oil", "Cheese", "Tomato", "Mineral Water" };
        decimal[] ProductPrice = new decimal[] { 3.4m, 22, 14, 2, 0.1m };
        List<Product> Products = new();
        for (int i = 0; i < ProductName.Length; i++)
        {
            Product tempProduct = new Product() { Name = ProductName[i], Price = ProductPrice[i], DateAdded = DateTime.Now };
            Products.Add(tempProduct);
        }

        await _context.Products.AddRangeAsync(Products);
        await _context.SaveChangesAsync();
    }



    public async Task SeedAll()
    {
        await SeedAdmin();
        await SeedUser();
        await SeedProducts();
    }

}
