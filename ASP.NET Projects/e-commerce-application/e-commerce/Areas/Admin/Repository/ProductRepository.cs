using e_commerce.DataAccess.Data;
using e_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Areas.Admin.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context=context;
            _context.Products.Include(x => x.category);
        }
        public void CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            Product product = GetProductById(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> GetAllProducts(string? includeProperties = null)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.ToList();
        }

        public Product GetProductById(int id, string? includeProperties = null)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty); // Keep adding Includes to the query
                }
            }
            Product? product = query.FirstOrDefault(p => p.Id == id);
            return product;
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
    public interface IProductRepository
    {
        public void CreateProduct(Product product);
        public void UpdateProduct(Product product);
        public void DeleteProduct(int id);
        public Product GetProductById(int id, string? includeProperties = null);
        public List<Product> GetAllProducts(string? includeProperties = null);
    }
}
