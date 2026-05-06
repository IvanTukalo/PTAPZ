using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Data
{
    public class LibraryDbContext : DbContext 
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}