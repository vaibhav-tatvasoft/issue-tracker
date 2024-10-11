using e_commerce.Data;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Models.Category> categoryList = _db._categories.ToList();
            return View(categoryList);
        }
    }
}
