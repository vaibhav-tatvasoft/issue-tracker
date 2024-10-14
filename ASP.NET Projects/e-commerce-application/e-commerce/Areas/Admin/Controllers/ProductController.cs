using e_commerce.Areas.Admin.Repository;
using e_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            List<Product> productList = _productRepository.GetAllCategories();
            return View(productList);
        }

        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.CreateProduct(product);
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Product? product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.UpdateProduct(product);
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Product? product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            if (ModelState.IsValid)
            {
                Product? product = _productRepository.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }
                _productRepository.DeleteProduct(id);
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
    }
}
