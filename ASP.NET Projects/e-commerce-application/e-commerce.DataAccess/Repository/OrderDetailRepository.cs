using e_commerce.DataAccess.Data;
using e_commerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace e_commerce.Areas.Admin.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void CreateOrderDetail(OrderDetail orderDetail)
        {
            _db.OrderDetails.Add(orderDetail);
            _db.SaveChanges();
        }

        public void DeleteOrderDetail(int id)
        {
            OrderDetail orderDetail = GetOrderDetailById(id);
            _db.OrderDetails.Remove(orderDetail);
            _db.SaveChanges();
        }
        public OrderDetail GetOrderDetailById(int id)
        {
            return _db.OrderDetails.FirstOrDefault(c => c.Id == id);
        }

        public OrderDetail Get(Expression<Func<OrderDetail, bool>> filter, string? includeProperties = null)
        {
            IQueryable<OrderDetail> query = _db.Set<OrderDetail>().Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
            _db.SaveChanges();
        }

        public List<OrderDetail> GetAllOrderDetails(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<OrderDetail> query = _db.Set<OrderDetail>();
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
    }

    public interface IOrderDetailRepository
    {
        public void CreateOrderDetail(OrderDetail orderDetail);
        public void DeleteOrderDetail(int id);
        public OrderDetail GetOrderDetailById(int id);
        public List<OrderDetail> GetAllOrderDetails(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null);
        public OrderDetail Get(Expression<Func<OrderDetail, bool>> filter, string? includeProperties = null);
        public void UpdateOrderDetail(OrderDetail orderDetail);
    }
}
