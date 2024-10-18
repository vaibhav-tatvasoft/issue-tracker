using e_commerce.DataAccess.Data;
using e_commerce.Models;
using System.Runtime.InteropServices;

namespace e_commerce.Areas.Admin.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void CreateCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            Category category = GetCategoryById(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }

        public List<Category> GetAllCategories()
        {
            return _db.Categories.ToList();

        }

        public Category GetCategoryById(int id)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCategory(Category category)
        {
            _db.Categories.Update(category);
            _db.SaveChanges();
        }
    }

    public interface ICategoryRepository
    {
        public void CreateCategory(Category category);
        public void UpdateCategory(Category category);
        public void DeleteCategory(int id);
        public Category GetCategoryById(int id);
        public List<Category> GetAllCategories();
    }
}
