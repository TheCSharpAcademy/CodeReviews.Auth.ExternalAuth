using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProductManagement.hasona23.Constants;
using ProductManagement.hasona23.Data;
using ProductManagement.hasona23.Services.Email;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
IConfigurationManager config = builder.Configuration;
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

EmailConfig emailConfig = builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>() ?? throw new InvalidOperationException("Email config section not found.");
builder.Services.AddSingleton<EmailConfig>(emailConfig);

builder.Host.UseSerilog((context, services, configuration) =>
{
    // Reads configuration settings for Serilog from the appsettings.json file or any other configuration source
    // This enables setting options such as log levels, sinks, and output formats directly from configuration files.
    configuration.ReadFrom.Configuration(context.Configuration);
});


builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    //options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = true;


    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.LogoutPath = "/Home/";
});
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = config["Authentication:Google:ClientId"];
        options.ClientSecret = config["Authentication:Google:ClientSecret"];
    })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = config["Authentication:Microsoft:ClientId"];
        options.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
    })
    .AddFacebook(options =>
    {
        options.ClientId = config["Authentication:Facebook:ClientId"];
        options.ClientSecret = config["Authentication:Facebook:ClientSecret"];
    })
    .AddTwitter(options =>
    {

        options.ConsumerKey = config["Authentication:Twitter:ConsumerApiKey"];
        options.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
    })
    .AddGitHub(options =>
    {
        options.ClientId = config["Authentication:Github:ClientId"];
        options.ClientSecret = config["Authentication:Github:ClientSecret"];
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
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = Roles.GetAllRoles();
    var context = services.GetRequiredService<ApplicationDbContext>();
    //context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    if (roles.Any())
    {
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));

            }
        }
    }
    DataSeeder.SeedBooks(context);
    await DataSeeder.SeedUsers(
        services.GetRequiredService<UserManager<IdentityUser>>());
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Books}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();