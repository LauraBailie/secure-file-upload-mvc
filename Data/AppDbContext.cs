using Microsoft.EntityFrameworkCore;
using LeakAlertDemo.Models;

namespace LeakAlertDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plumber> Plumbers { get; set; }
    }
}