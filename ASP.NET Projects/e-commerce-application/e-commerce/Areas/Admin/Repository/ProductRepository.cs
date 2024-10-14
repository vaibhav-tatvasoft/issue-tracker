using e_commerce.DataAccess.Data;
using e_commerce.Models;

namespace e_commerce.Areas.Admin.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context=context;
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

        public List<Product> GetAllCategories()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.Id == id);
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
        public Product GetProductById(int id);
        public List<Product> GetAllCategories();
    }
}
