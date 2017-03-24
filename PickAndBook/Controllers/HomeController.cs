using Bytes2you.Validation;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;

namespace PickAndBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public HomeController(ICategoryRepository categoryRepository)
        {
            Guard.WhenArgument(categoryRepository, nameof(categoryRepository)).IsNull().Throw();

            this.categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        [OutputCache(Duration = 3600, VaryByParam = "none", VaryByCustom = "CustomSqlDependency")]
        public ActionResult HomeCategories()
        {
            var categories = this.categoryRepository.All().ToList();
            // this.ViewBag.Current = DateTime.Now;
            return PartialView("_HomeCategories", categories);
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public ViewResult Error()
        {
            return this.View("Error");
        }
    }
}