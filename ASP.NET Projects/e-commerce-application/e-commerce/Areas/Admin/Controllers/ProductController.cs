using e_commerce.Areas.Admin.Repository;
using e_commerce.Models;
using e_commerce.Models.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> productList = _productRepository.GetAllProducts();
            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM obj = new ProductVM();
            IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAllCategories().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            obj.CategoryList = categoryList;
            if (id == null)
            {
                //create method
                obj.product = new Product();
            }
            else
            {
                if (id == 0)
                {
                    return NotFound();
                }
                Product? product = _productRepository.GetProductById((int)id);
                obj.product = product;
                if (product == null)
                {
                    return NotFound();
                }
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productvm, IFormFile file)
        {
            if (ModelState.IsValid )
            {
                if(productvm.product.Id == null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\product");

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productvm.product.ImageUrl = @"images\product/" + fileName;
                    }
                    _productRepository.CreateProduct(productvm.product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _productRepository.UpdateProduct(productvm.product);
                    TempData["success"] = "Product updated successfully";
                }
                return RedirectToAction("Index", "Product");
            }
            return View(productvm);
        }

        //public IActionResult Edit(int id)
        //{
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? product = _productRepository.GetProductById(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _productRepository.UpdateProduct(product);
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}
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
