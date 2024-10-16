using e_commerce.Areas.Admin.Repository;
using e_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace e_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository productRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = productRepository.GetAllProducts(includeProperties: "category");
            return View(products);
        }
        public IActionResult Details(int productId)
        {
            Product product = productRepository.GetProductById(productId, includeProperties: "category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}