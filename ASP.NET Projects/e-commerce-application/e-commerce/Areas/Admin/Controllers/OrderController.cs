using e_commerce.Areas.Admin.Repository;
using e_commerce.Models;
using e_commerce.Models.Models.ViewModels;
using e_commerce.Utility;
using Microsoft.AspNetCore.Mvc;
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
