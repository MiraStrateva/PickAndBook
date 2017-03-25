using PickAndBook.Data;
using PickAndBook.Data.Repositories.Contracts;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PickAndBook.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICategoryRepository categoryRepository;

        public HomeController(IPickAndBookData data)
            :base(data)
        {
            this.categoryRepository = data.Categories;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            // throw new HttpException(404, "Test error behavior");
            return View();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        public ViewResult Error()
        {
            return this.View("Error");
        }
    }
}