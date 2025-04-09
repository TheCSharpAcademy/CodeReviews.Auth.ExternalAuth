using Microsoft.AspNetCore.Identity;
using ProductManagement.hasona23.Constants;
using ProductManagement.hasona23.Models;

namespace ProductManagement.hasona23.Data;

public static class DataSeeder
{
    public static void SeedBooks(ApplicationDbContext context)
    {
        if (context.Books.Any())
            return;
        for (int i = 1; i <= 64; i++)
        {
            context.Books.Add(new BookModel
            {
                Name = $"Book {i}",
                Price = new Random().Next(2,20),
                DateAdded = DateTime.Now.AddDays(-new Random().Next(100)),
                IsActive = i % 2 == 0,
            });
        }
        context.SaveChanges();
    }

    public static async Task SeedUsers(UserManager<IdentityUser> userManager)
    {
        var adminEmail = "admin@example.com";
        var adminPassword = "Admin123!";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }

        // Seed Staff Users
        for (int i = 1; i <= 10; i++)
        {
            var staffEmail = $"staff{i}@example.com";
            var staffPassword = "Staff123!";
            if (await userManager.FindByEmailAsync(staffEmail) == null)
            {
                var staff = new IdentityUser { UserName = staffEmail, Email = staffEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(staff, staffPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(staff, Roles.Staff);
                }
            }
        }

        // Seed Customer Users
        for (int i = 1; i <= 20; i++)
        {
            var customerEmail = $"customer{i}@example.com";
            var customerPassword = "Customer123!";
            if (await userManager.FindByEmailAsync(customerEmail) == null)
            {
                var customer = new IdentityUser { UserName = customerEmail, Email = customerEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(customer, customerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, Roles.Customer);
                }
            }
        }
    }
}