using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QuizGame.API;
using QuizGame.Data;
using QuizGame.Data.Models;
using QuizGame.Data.Repositories;
using QuizGame.Data.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
        policyBuilder =>
        {
            policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
        });
});

builder.Services.AddHttpClient();

builder.Services.AddDbContext<QuizGameContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; })
    .AddEntityFrameworkStores<QuizGameContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = "QuizGameToken";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthentication();

builder.Services.AddScoped<IQuizzesRepository, QuizzesRepository>();
builder.Services.AddScoped<IRepository<Question>, Repository<Question>>();
builder.Services.AddScoped<IRepository<Answer>, Repository<Answer>>();
builder.Services.AddScoped<IGamesRepository, GamesRepository>();

builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IQuizzesService, QuizzesService>();
builder.Services.AddScoped<IErrorsService, ErrorsService>();
builder.Services.AddScoped<ILogger, ErrorLogger>();

builder.Services.AddIdentityApiEndpoints<User>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("policy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();