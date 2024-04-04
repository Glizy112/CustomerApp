using Microsoft.EntityFrameworkCore;
using WebApplicationMVCExample.Models;

namespace WebApplicationMVCExample.DataContext
{
    public class AppDBContext : DbContext
    { 
        public AppDBContext(DbContextOptions options):base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}
