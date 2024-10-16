using e_commerce.DataAccess.Data;
using e_commerce.Models;
using static e_commerce.Areas.Admin.Repository.CompanyRepository;

namespace e_commerce.Areas.Admin.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }

        public Company GetCompanyById(int id)
        {
            return _context.Companies.FirstOrDefault(c => c.Id == id);
        }

        public void AddCompany(Company company)
        {
            _context.Companies.Add(company);
        }

        public void UpdateCompany(Company company)
        {
            _context.Companies.Update(company);
        }

        public void DeleteCompany(int id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
        public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies();
        Company GetCompanyById(int id);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(int id);
        void Save();
    }

}
