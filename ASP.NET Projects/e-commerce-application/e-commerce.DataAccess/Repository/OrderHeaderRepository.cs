using e_commerce.DataAccess.Data;
using e_commerce.Models;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipes;
using System.Linq.Expressions;

namespace e_commerce.Areas.Admin.Repository
{
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void CreateOrderHeader(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Add(orderHeader);
            _db.SaveChanges();
        }

        public void DeleteOrderHeader(int id)
        {
            OrderHeader orderHeader = GetOrderHeaderById(id);
            _db.OrderHeaders.Remove(orderHeader);
            _db.SaveChanges();
        }
        public OrderHeader GetOrderHeaderById(int id)
        {
            return _db.OrderHeaders.FirstOrDefault(c => c.Id == id);
        }

        public OrderHeader Get(Expression<Func<OrderHeader, bool>> filter, string? includeProperties = null)
        {
            IQueryable<OrderHeader> query = _db.Set<OrderHeader>().Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public void UpdateOrderHeader(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
            _db.SaveChanges();
        }

        public List<OrderHeader> GetAllOrderHeaders(Expression<Func<OrderHeader, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<OrderHeader> query = _db.Set<OrderHeader>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.ToList();

        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
            _db.SaveChanges();
        }

        public void UpdateStripePaymentId(int id, string SessionId, string paymentIntentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.SessionId = SessionId;
                orderFromDb.PaymentIntentId = paymentIntentId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
            _db.SaveChanges();
        }
    }

    public interface IOrderHeaderRepository
    {
        public void CreateOrderHeader(OrderHeader orderHeader);
        public void DeleteOrderHeader(int id);
        public OrderHeader GetOrderHeaderById(int id);
        public List<OrderHeader> GetAllOrderHeaders(Expression<Func<OrderHeader, bool>>? filter = null, string? includeProperties = null);
        public OrderHeader Get(Expression<Func<OrderHeader, bool>> filter, string? includeProperties = null);
        public void UpdateOrderHeader(OrderHeader orderHeader);
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus);
        public void UpdateStripePaymentId(int id, string SessionId, string paymentIntentId);

	}
}
