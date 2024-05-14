using Microsoft.EntityFrameworkCore;
using Movies.StevieTV.Models;

namespace Movies.StevieTV.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext (DbContextOptions<MoviesContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<ErrorLog> ErrorLog { get; set; }
    }
}
