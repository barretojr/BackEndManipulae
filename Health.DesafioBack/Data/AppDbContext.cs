using Health.DesafioBack.Models;
using Microsoft.EntityFrameworkCore;

namespace Health.DesafioBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Video> Videos { get; set; }
    }
}
