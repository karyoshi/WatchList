using Microsoft.EntityFrameworkCore;
using WatchListMVC.Models;

namespace WatchListMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {              
        }

        public DbSet<MovieModel> Movies { get; set; }

    }
}
