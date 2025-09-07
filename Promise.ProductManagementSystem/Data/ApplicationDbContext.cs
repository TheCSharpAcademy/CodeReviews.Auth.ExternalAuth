using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Promise.ProductManagementSystem.Models;

namespace Promise.ProductManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>().Property(p => p.DateAdded).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AuditLog>().HasKey(a => a.Id);

            // seed roles: Admin and Staff

            var adminRoleId = "d89237d7-7b1e-4196-b7e1-1a4efe32ff4c";
            var staffRoleId = "bdcc17b0-38dd-4091-8273-388b0571ed70";
            var adminUserId = "ace72f65-ca2d-4dac-8f11-1096ad1dff42";
            var adminUser2Id = "8d6a0f5a-b3dc-41f4-9d73-748f04df2d48";   
            var staff1Id = "f4e1b5e1-3c3e-4c6a-9f7b-2d5e6c8a9b10";  
            var staff2Id = "a1b2c3d4-e5f6-7890-abcd-ef1234567890";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id= adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = staffRoleId, Name = "Staff", NormalizedName = "STAFF", ConcurrencyStamp = Guid.NewGuid().ToString() }
            );

            var hasher = new PasswordHasher<IdentityUser>();
            // seed an admin user
            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = "adminuser@gmail.com",
                NormalizedUserName = "ADMINUSER@GMAIL.COM",
                Email = "adminuser@gmail.com",
                NormalizedEmail = "ADMINUSER@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            // seed two staff users
            var staff1 = new IdentityUser
            {
                Id = staff1Id,
                UserName = "staffone@gmail.com",
                NormalizedUserName = "STAFFONE@GMAIL.COM",
                Email = "staffone@gmail.com",
                NormalizedEmail = "STAFFONE@GMAIL.COM",
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var staff2 = new IdentityUser
            {
                Id = staff2Id,
                UserName = "stafftwo@gmail.com",
                NormalizedUserName = "STAFFTWO@GMAIL.COM",
                Email = "stafftwo@gmail.com",
                NormalizedEmail = "STAFFTWO@GMAIL.COM",
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            modelBuilder.Entity<IdentityUser>().HasData(adminUser, staff1, staff2);
            // assign admin user to Admin role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = staffRoleId,
                    UserId = staff1Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = staffRoleId,
                    UserId = staff2Id
                }
            );
        }
    }
}
