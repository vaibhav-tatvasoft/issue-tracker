using e_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> _categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id=1, Name = "something", DisplayOrder = "1" });
        }
    }
}
