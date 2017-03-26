using System.Web;
using System.Web.Mvc;
using System.Linq;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Data.Common;
using PickAndBook.Helpers.Contracts;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace PickAndBook.Controllers
{
    [Authorize(Roles = DataConstants.ClientRoleName)]
    public class RegisterCompanyController : BaseController
    {
        private IFileUploader fileUploader;
        private IUserRoleManager userRoleManager;

        public RegisterCompanyController(IPickAndBookData data) 
            : base(data)
        {
        }

        public RegisterCompanyController(IPickAndBookData data, IFileUploader fileUploader, IUserRoleManager userRoleManager)
            : base(data)
        {
            this.fileUploader = fileUploader;
            this.userRoleManager = userRoleManager;
        }

        public ActionResult Index()
        {
            Company currentCompany = this.Data.Companies.GetCompanyByUserId(User.Identity.GetUserId());
            if (currentCompany != null)
            {
                return View("CompanyDetails", currentCompany);
            }

            currentCompany = new Company();
            ViewBag.CategoryId = new SelectList(this.Data.Categories.All().ToList(), "CategoryId", "CategoryName");
            return View(currentCompany);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Company company, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string uploadedImage = this.fileUploader.UploadFile(upload, Common.Constants.CompaniesImageFolder);
                    if (uploadedImage == "")
                    {
                        ModelState.AddModelError(Common.Constants.FileCannotBeUploadedKey, Common.Constants.FileCannotBeUploaded);
                        return View(company);
                    }
                    company.CompanyImage = uploadedImage;
                }

                this.Data.Companies.Add(company);
                this.Data.SaveChanges();

                var llRoleChanged = await this.userRoleManager.ChangeUserRoleFromClientToCompany(HttpContext, User);
                if (llRoleChanged)
                {
                    return View("CompanyDetails", company);
                }
                ModelState.AddModelError("Roles", "User roles not assigned!");
            }

            ViewBag.CategoryId = new SelectList(this.Data.Categories.All().ToList(), "CategoryId", "CategoryName", company.CategoryId);
            return View(company);
        }
    }
}