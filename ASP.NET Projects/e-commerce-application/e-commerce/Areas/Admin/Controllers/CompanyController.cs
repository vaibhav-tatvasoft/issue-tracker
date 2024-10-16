using e_commerce.Areas.Admin.Repository;
using e_commerce.Models.Models.ViewModels;
using e_commerce.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using e_commerce.Models.Models;

namespace e_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> companyList = _companyRepository.GetAllCompanies();
            return View(companyList);
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id != null)
            {
                company = _companyRepository.GetCompanyById((int)id);
                if (id == 0)
                {
                    return NotFound();
                }
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == null || company.Id == 0)
                {

                    _companyRepository.AddCompany(company);
                    _companyRepository.Save();
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _companyRepository.UpdateCompany(company);
                    _companyRepository.Save();
                    TempData["success"] = "Company updated successfully";
                }
                return RedirectToAction("Index", "Company");
            }
            return View(company);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Company? company = _companyRepository.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            if (ModelState.IsValid)
            {
                Company? company = _companyRepository.GetCompanyById(id);
                if (company == null)
                {
                    return NotFound();
                }
                _companyRepository.DeleteCompany(id);
                _companyRepository.Save();
                TempData["success"] = "Company deleted successfully";
                return RedirectToAction("Index", "Company");
            }
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Company> companyList = _companyRepository.GetAllCompanies();
            return Json(new { data = companyList });
        }
        #endregion
    }
}
