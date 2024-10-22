using e_commerce.DataAccess.Data;
using e_commerce.Models;
using e_commerce.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Runtime.InteropServices;

namespace e_commerce.Areas.Admin.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public void CreateApplicationUser(ApplicationUser applicationUser)
        {
            _db.ApplicationUsers.Add(applicationUser);
            _db.SaveChanges();
        }

        public void DeleteApplicationUser(string id)
        {
            ApplicationUser applicationUser = GetApplicationUserById(id);
            _db.ApplicationUsers.Remove(applicationUser);
            _db.SaveChanges();
        }

        public ApplicationUser Get(Expression<Func<ApplicationUser, bool>> filter)
        {
            IQueryable<ApplicationUser> query = _db.Set<ApplicationUser>().Where(filter);
            return query.FirstOrDefault();
        }

        public void UpdateApplicationUser(ApplicationUser applicationUser)
        {
            _db.ApplicationUsers.Update(applicationUser);
            _db.SaveChanges();
        }

        public List<ApplicationUser> GetAllApplicationUsers(Expression<Func<ApplicationUser, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<ApplicationUser> query = _db.Set<ApplicationUser>();
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

        public ApplicationUser GetApplicationUserById(string id)
        {
            return _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
        }
    }

    public interface IApplicationUserRepository
    {
        public void CreateApplicationUser(ApplicationUser applicationUser);
        public void DeleteApplicationUser(string id);
        public ApplicationUser GetApplicationUserById(string id);
        public List<ApplicationUser> GetAllApplicationUsers(Expression<Func<ApplicationUser, bool>>? filter = null, string? includeProperties = null);
        public ApplicationUser Get(Expression<Func<ApplicationUser, bool>> filter);
    }
}
