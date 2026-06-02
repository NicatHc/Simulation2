using Microsoft.EntityFrameworkCore;
using Simulation2.Models;

namespace Simulation2.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Worker> workers { get; set; }




    }
}
