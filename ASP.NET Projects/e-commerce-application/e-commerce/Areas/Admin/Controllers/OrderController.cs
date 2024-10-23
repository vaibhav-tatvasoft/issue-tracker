using e_commerce.Areas.Admin.Repository;
using e_commerce.Models;
using e_commerce.Models.Models.ViewModels;
using e_commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Security.Claims;

namespace e_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHeaderRepopsitory;
        private readonly IOrderDetailRepository _orderDetailRepopsitory;

        public OrderController(IOrderHeaderRepository orderHeaderRepopsitory, IOrderDetailRepository orderDetailRepopsitory)
        {
            _orderHeaderRepopsitory = orderHeaderRepopsitory;
            _orderDetailRepopsitory = orderDetailRepopsitory;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId)
        {
            OrderVM order = new()
            {
                orderHeader = _orderHeaderRepopsitory.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                orderDetail = _orderDetailRepopsitory.GetAllOrderDetails(u => u.OrderHeaderId == orderId, includeProperties: "Product"),
            };

            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetail(OrderVM orderVm)
        {
            var orderFromDb = _orderHeaderRepopsitory.GetOrderHeaderById(orderVm.orderHeader.Id);

            orderFromDb.Name = orderVm.orderHeader.Name;
            orderFromDb.PhoneNumber = orderVm.orderHeader.PhoneNumber;
            orderFromDb.StreetAddress = orderVm.orderHeader.StreetAddress;
            orderFromDb.City = orderVm.orderHeader.City;
            orderFromDb.State = orderVm.orderHeader.State;
            orderFromDb.PostalCode = orderVm.orderHeader.PostalCode;
            orderFromDb.Carrier = orderVm.orderHeader.Carrier;
            orderFromDb.TrackingNumber = orderVm.orderHeader.TrackingNumber;

            _orderHeaderRepopsitory.UpdateOrderHeader(orderFromDb);
            TempData["success"] = "Order details updated!";

            return RedirectToAction("Details", new { orderId = orderVm.orderHeader.Id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing(OrderVM orderVm)
        {
            var orderFromDb = _orderHeaderRepopsitory.GetOrderHeaderById(orderVm.orderHeader.Id);
            _orderHeaderRepopsitory.UpdateStatus(orderVm.orderHeader.Id, SD.StatusInProcess, orderFromDb.PaymentStatus);
            TempData["success"] = "Order started processing!";

            return RedirectToAction("Details", new { orderId = orderVm.orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder(OrderVM orderVm)
        {
            var orderFromDb = _orderHeaderRepopsitory.GetOrderHeaderById(orderVm.orderHeader.Id);
            _orderHeaderRepopsitory.UpdateStatus(orderVm.orderHeader.Id, SD.StatusShipped, orderFromDb.PaymentStatus);

            orderFromDb.Carrier = orderVm.orderHeader.Carrier;
            orderFromDb.TrackingNumber = orderVm.orderHeader.TrackingNumber;
            orderFromDb.ShippingDate = DateTime.Now;
            if (orderFromDb.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderFromDb.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            _orderHeaderRepopsitory.UpdateOrderHeader(orderFromDb);

            TempData["success"] = "Order shipped!";

            return RedirectToAction("Details", new { orderId = orderVm.orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder(OrderVM orderVm)
        {
            var orderFromDb = _orderHeaderRepopsitory.GetOrderHeaderById(orderVm.orderHeader.Id);
            if (orderFromDb.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = orderFromDb.PaymentIntentId
                };

                var service = new RefundService();
                service.Create(options);
                orderFromDb.OrderStatus = SD.StatusCancelled;
                orderFromDb.PaymentStatus = SD.StatusRefunded;
            }
            else
            {
                orderFromDb.OrderStatus = SD.StatusCancelled;
                orderFromDb.PaymentStatus = SD.StatusCancelled;
            }
            _orderHeaderRepopsitory.UpdateOrderHeader(orderFromDb);

            TempData["success"] = "Order Cancelled!";

            return RedirectToAction("Details", new { orderId = orderVm.orderHeader.Id });
        }

        [ActionName("Details")]
        [HttpPost]
        public IActionResult Details_PAY_NOW(OrderVM orderVm)
        {
            orderVm.orderHeader = _orderHeaderRepopsitory
                .Get(u => u.Id == orderVm.orderHeader.Id, includeProperties: "ApplicationUser");
            orderVm.orderDetail = _orderDetailRepopsitory
                .GetAllOrderDetails(u => u.OrderHeaderId == orderVm.orderHeader.Id, includeProperties: "Product");

            //stripe logic
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={orderVm.orderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={orderVm.orderHeader.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in orderVm.orderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            _orderHeaderRepopsitory.UpdateStripePaymentId(orderVm.orderHeader.Id, session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders;


            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders = _orderHeaderRepopsitory.GetAllOrderHeaders(includeProperties: "ApplicationUser").ToList();
            }
            else
            {

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _orderHeaderRepopsitory
                    .GetAllOrderHeaders(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            }


            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;

            }


            return Json(new { data = objOrderHeaders });
        }


        #endregion
    }

}
