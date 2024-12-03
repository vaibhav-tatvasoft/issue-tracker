using ChattingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApplication.DataAccess.Repository
{
    public class UserRepository  : IUserRepository
    {
        private readonly ApplicationDBContext _db;

        public UserRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public void AddUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
        }

        public IQueryable<User> GetAllUsers(Expression<Func<User, bool>> filter)
        {
            IQueryable<User> query = _db.Set<User>();
            if (filter != null)
            {
                query = query.Where(filter).Include(u => u.groups).AsNoTracking();
            }
            return query;
        }

        public async Task<User> GetUser(Expression<Func<User, bool>> filter)
        {
            IQueryable<User> query = _db.Set<User>();
            if (filter != null)
            {
                query = query.Where(filter).Include(u => u.groups);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public void UpdateUser(User user)
        {
            _db.Users.Update(user);
            //_db.SaveChanges();
        }
    }

    public interface IUserRepository
    {
        public void AddUser(User user);
        public IQueryable<User> GetAllUsers(Expression<Func<User, bool>> filter);
        public Task<User> GetUser(Expression<Func<User, bool>> filter);
        public void UpdateUser(User user);
        public void DeleteUser(User user);
    }
}
