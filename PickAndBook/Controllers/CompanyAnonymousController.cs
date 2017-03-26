using System;
using System.Web.Mvc;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using System.Linq;

namespace PickAndBook.Controllers
{
    [AllowAnonymous]
    public class CompanyAnonymousController : BaseController
    {
        public CompanyAnonymousController(IPickAndBookData data) 
            : base(data)
        {
        }

        // GET: CompanyAnonymous
        public ActionResult Index()
        {
            var companies = this.Data.Companies.All().ToList();
            return View(companies);
        }

        public ActionResult ByCategory(Guid? categoryId)
        {
            var companies = this.Data.Companies.GetCompaniesByCategoryId(categoryId);
            if (companies == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Index", companies.ToList());
        }

        public ActionResult Detail(Guid? companyId)
        {
            Company currentCompany = this.Data.Companies.GetById(companyId);
            if (currentCompany == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(currentCompany);
        }
    }
}