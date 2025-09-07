using System;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.OAuth;
using NuGet.Configuration;
using Promise.ProductManagementSystem.Data;
using Promise.ProductManagementSystem.Helpers;
using Promise.ProductManagementSystem.Middleware;
using Promise.ProductManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    // User settings
    options.User.RequireUniqueEmail = true;

    // Account confirmation settings
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = true;

});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;

    // Authentication paths
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDatabaseLogger, DatabaseLogger>();
builder.Services.AddSingleton<EmailQueue>();
builder.Services.AddHostedService<BackgroundEmailService>();

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        googleOptions.SaveTokens = true;

        googleOptions.Events = new OAuthEvents
        {
            OnRemoteFailure = ctx =>
            {
                var message = Uri.EscapeDataString(ctx.Failure?.Message ?? "External provider error");
                ctx.Response.Redirect($"/Identity/Account/Login?remoteError={message}");
                ctx.HandleResponse();
                return Task.CompletedTask;
            }
        };
    })
    .AddTwitter(twitterOptions =>
    {
        twitterOptions.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerAPIKey"];
        twitterOptions.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"];  
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
        facebookOptions.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
        facebookOptions.SaveTokens = true;
        facebookOptions.Events = new OAuthEvents
        {
            OnRemoteFailure = ctx =>
            {
                var message = Uri.EscapeDataString(ctx.Failure?.Message ?? "External provider error");
                ctx.Response.Redirect($"/Identity/Account/Login?remoteError={message}");
                ctx.HandleResponse();
                return Task.CompletedTask;
            }
        };
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
        microsoftOptions.SaveTokens = true;
        microsoftOptions.Events = new OAuthEvents
        {
            OnRemoteFailure = ctx =>
            {
                var message = Uri.EscapeDataString(ctx.Failure?.Message ?? "External provider error");
                ctx.Response.Redirect($"/Identity/Account/Login?remoteError={message}");
                ctx.HandleResponse();
                return Task.CompletedTask;
            }
        };
    })
    .AddGitHub(githubOptions =>
    {
        githubOptions.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
        githubOptions.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];

        // Request email scope explicitly 
        githubOptions.Scope.Add("user:email");
        githubOptions.SaveTokens = true;

        githubOptions.Events = new OAuthEvents
        {
            OnRemoteFailure = ctx =>
            {
                var message = Uri.EscapeDataString(ctx.Failure?.Message ?? "External provider error");
                ctx.Response.Redirect($"/Identity/Account/Login?remoteError={message}");
                ctx.HandleResponse();
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();


    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        // Use EnsureCreated to automatically create database and tables
        var created = dbContext.Database.EnsureCreated();

        if (created)
        {
            logger.LogInformation("Database was created successfully using EnsureCreated.");
        }
        else
        {
            logger.LogInformation("Database already exists.");
        }

    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while ensuring the database was created.");
        throw; 
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ErrorLoggingMiddleware>();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
