using e_commerce.Areas.Admin.Repository;
using e_commerce.Models;
using e_commerce.Models.Models.ViewModels;
using e_commerce.Utility;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using static e_commerce.Areas.Admin.Repository.OrderHeaderRepository;

namespace e_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public CartController(IShoppingCartRepository shoppingCartRepository, IApplicationUserRepository applicationUserRepository, IOrderHeaderRepository orderHeaderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _applicationUserRepository = applicationUserRepository;
            _orderHeaderRepository = orderHeaderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _shoppingCartRepository.GetAllShoppingCarts(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cartItem in shoppingCartVM.ShoppingCartList)
            {
                cartItem.Price = GetPriceByOrderQuantity(cartItem);
                shoppingCartVM.OrderHeader.OrderTotal += cartItem.Price * cartItem.Count;
            }

            return View(shoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _shoppingCartRepository.GetAllShoppingCarts(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cartItem in shoppingCartVM.ShoppingCartList)
            {
                cartItem.Price = GetPriceByOrderQuantity(cartItem);
                shoppingCartVM.OrderHeader.OrderTotal += cartItem.Price * cartItem.Count;
            }
            shoppingCartVM.OrderHeader.ApplicationUser = _applicationUserRepository.Get(x => x.Id == userId);
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.Phone;
            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
            shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

            return View(shoppingCartVM);
        }

        [HttpPost]
        public IActionResult Summary(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCartVM.ShoppingCartList = _shoppingCartRepository.GetAllShoppingCarts(u => u.ApplicationUserId == userId,
                includeProperties: "Product");
            shoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _applicationUserRepository.Get(u => u.Id == userId);

            //calculate total
            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceByOrderQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;

            }
            else
            {
                //it is a company user
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            _orderHeaderRepository.CreateOrderHeader(shoppingCartVM.OrderHeader);

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _orderDetailRepository.CreateOrderDetail(orderDetail);
            }


            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                var domain = "https://localhost:7051/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + "Customer/Cart/Index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var order in shoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(order.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = order.Product.Title
                            }
                        },
                        Quantity = order.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                _orderHeaderRepository.UpdateStripePaymentId(shoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            var orderHeader = _orderHeaderRepository.Get(u => u.Id == id);

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    orderHeader.PaymentIntentId = session.PaymentIntentId;
                    _orderHeaderRepository.UpdateStatus(orderHeader.Id, SD.StatusApproved, SD.PaymentStatusApproved);
                }
            }

            List<ShoppingCart> shoppingCarts = _shoppingCartRepository.GetAllShoppingCarts(u => u.ApplicationUserId == orderHeader.ApplicationUserId);
            foreach (var cart in shoppingCarts)
            {
                _shoppingCartRepository.DeleteShoppingCart(cart.Id);
            }

            return View(id);
        }


        public IActionResult Plus(int id)
        {
            if (id != 0)
            {
                ShoppingCart shoppingCart = _shoppingCartRepository.GetShoppingCartById(id);
                shoppingCart.Count += 1;
                _shoppingCartRepository.UpdateShoppingCart(shoppingCart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int id)
        {
            if (id != 0)
            {
                ShoppingCart shoppingCart = _shoppingCartRepository.GetShoppingCartById(id);
                shoppingCart.Count -= 1;
                if (shoppingCart.Count == 0)
                {
                    _shoppingCartRepository.DeleteShoppingCart(id);
                }
                else
                {
                    _shoppingCartRepository.UpdateShoppingCart(shoppingCart);
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                _shoppingCartRepository.DeleteShoppingCart(id);
            }
            return RedirectToAction("Index");
        }

        private double GetPriceByOrderQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count < 51)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count > 50 && shoppingCart.Count < 101)
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
