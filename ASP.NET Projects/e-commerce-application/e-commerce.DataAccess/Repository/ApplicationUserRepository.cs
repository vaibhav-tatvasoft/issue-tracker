using e_commerce.DataAccess.Data;
using e_commerce.Models;
using e_commerce.Models.Models;
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
        public void CreateApplicationUser(ApplicationUser shoppingCart)
        {
            _db.ApplicationUsers.Add(shoppingCart);
            _db.SaveChanges();
        }

        public void DeleteApplicationUser(string id)
        {
            ApplicationUser shoppingCart = GetApplicationUserById(id);
            _db.ApplicationUsers.Remove(shoppingCart);
            _db.SaveChanges();
        }

        public List<ApplicationUser> GetAllApplicationUsers()
        {
            return _db.ApplicationUsers.ToList();

        }

        public ApplicationUser GetApplicationUserById(string id)
        {
            return _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
        }
    }

    public interface IApplicationUserRepository
    {
        public void CreateApplicationUser(ApplicationUser shoppingCart);
        public void DeleteApplicationUser(string id);
        public ApplicationUser GetApplicationUserById(string id);
        public List<ApplicationUser> GetAllApplicationUsers();
    }
}
