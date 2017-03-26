using PickAndBook.Data;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PickAndBook.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ICompanyRepository companyRepository;

        public HomeController(IPickAndBookData data)
            :base(data)
        {
            this.categoryRepository = data.Categories;
            this.companyRepository = data.Companies;
        }

        public ActionResult Index()
        {
            // throw new HttpException(404, "Test error behavior");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [OutputCache(Duration = 3600, VaryByParam = "none", VaryByCustom = "CustomSqlDependency")]
        public ActionResult HomeCategories()
        {
            var categories = this.categoryRepository.All().ToList();
            // this.ViewBag.Current = DateTime.Now;
            return PartialView("_HomeCategories", categories);
        }

        public ActionResult LastAddedCompanies()
        {
            var companies = this.companyRepository.GetLastAddedCompanies().ToList();
            this.ViewBag.Current = DateTime.Now;
            return PartialView("_LastAddedCompanies", companies);
        }

        public ViewResult Error()
        {
            return this.View("Error");
        }
    }
}