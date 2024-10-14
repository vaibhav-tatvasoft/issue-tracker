using e_commerce.Models;
using e_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "something", DisplayOrder = "1" });
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Clean Code",
                    Description = "A Handbook of Agile Software Craftsmanship",
                    ISBN = "9780132350884",
                    Author = "Robert C. Martin",
                    ListPrice = 50.00,
                    Price = 45.00,
                    Price50 = 40.00,
                    Price100 = 35.00
                },
            new Product
            {
                Id = 2,
                Title = "The Pragmatic Programmer",
                Description = "Your Journey to Mastery",
                ISBN = "9780135957059",
                Author = "Andrew Hunt, David Thomas",
                ListPrice = 60.00,
                Price = 55.00,
                Price50 = 50.00,
                Price100 = 45.00
            },
            new Product
            {
                Id = 3,
                Title = "Refactoring",
                Description = "Improving the Design of Existing Code",
                ISBN = "9780201485677",
                Author = "Martin Fowler",
                ListPrice = 70.00,
                Price = 65.00,
                Price50 = 60.00,
                Price100 = 55.00
            });
        }
    }
}
