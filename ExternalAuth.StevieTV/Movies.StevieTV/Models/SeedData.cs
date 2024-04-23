using Microsoft.EntityFrameworkCore;
using Movies.StevieTV.Data;
using Movies.StevieTV.Areas.Identity.Data;

namespace Movies.StevieTV.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new MoviesContext(serviceProvider.GetRequiredService<DbContextOptions<MoviesContext>>());

        context.Database.Migrate();

        if (!context.Movie.Any())
        {


            context.Movie.AddRange(
                new Movie()
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M,
                    Rating = "R"
                },
                new Movie
                {
                    Title = "Ghostbusters",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M,
                    Rating = "PG"
                },
                new Movie
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M,
                    Rating = "PG"
                },
                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M,
                    Rating = "U"
                }
            );
        }

        context.SaveChanges();
        
        using var appContext = new ApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>());

        appContext.Database.Migrate();
        appContext.SaveChanges();
    }
}