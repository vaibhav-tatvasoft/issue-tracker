using e_commerce.DataAccess.Data;
using e_commerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace e_commerce.Areas.Admin.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void CreateShoppingCart(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Add(shoppingCart);
            _db.SaveChanges();
        }

        public void DeleteShoppingCart(int id)
        {
            ShoppingCart shoppingCart = GetShoppingCartById(id);
            _db.ShoppingCarts.Remove(shoppingCart);
            _db.SaveChanges();
        }
        public ShoppingCart GetShoppingCartById(int id)
        {
            return _db.ShoppingCarts.FirstOrDefault(c => c.Id == id);
        }

        public ShoppingCart Get(Expression<Func<ShoppingCart, bool>> filter)
        {
            IQueryable<ShoppingCart> query = _db.Set<ShoppingCart>().Where(filter);
            return query.FirstOrDefault();
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Update(shoppingCart);
            _db.SaveChanges();
        }

        public List<ShoppingCart> GetAllShoppingCarts(Expression<Func<ShoppingCart, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable <ShoppingCart> query = _db.Set<ShoppingCart>();
            if(filter != null)
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

    public interface IShoppingCartRepository
    {
        public void CreateShoppingCart(ShoppingCart shoppingCart);
        public void DeleteShoppingCart(int id);
        public ShoppingCart GetShoppingCartById(int id);
        public List<ShoppingCart> GetAllShoppingCarts(Expression<Func<ShoppingCart,bool>>? filter = null, string? includeProperties = null);
        public ShoppingCart Get(Expression<Func<ShoppingCart, bool>> filter);
        public void UpdateShoppingCart(ShoppingCart shoppingCart);
    }
}
