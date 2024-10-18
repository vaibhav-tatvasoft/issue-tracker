using e_commerce.Areas.Admin.Repository;
using e_commerce.Models;
using e_commerce.Models.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace e_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public CartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _shoppingCartRepository.GetAllShoppingCarts(u => u.ApplicationUserId == userId, includeProperties: "Product"),
            };
            foreach (var cartItem in shoppingCartVM.ShoppingCartList)
            {
                cartItem.Price = GetPriceByOrderQuantity(cartItem);
                shoppingCartVM.OrderTotal += cartItem.Price * cartItem.Count;
            }

                return View(shoppingCartVM);
        }

        private double GetPriceByOrderQuantity(ShoppingCart shoppingCart)
        {
            if(shoppingCart.Count < 51)
            {
                return shoppingCart.Product.Price;
            }
            else if(shoppingCart.Count > 50 && shoppingCart.Count < 101)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
    }
}
