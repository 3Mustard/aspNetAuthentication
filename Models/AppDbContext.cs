using Microsoft.EntityFrameworkCore;
using aspNetAuth.Models;

namespace aspNetAuth
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}