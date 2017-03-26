using System.Web.Mvc;
using PickAndBook.Data;
using PickAndBook.Data.Common;
using PickAndBook.Data.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using PickAndBook.Helpers.Contracts;

namespace PickAndBook.Controllers
{
    [Authorize(Roles = DataConstants.CompanyRoleName)]
    public class CompanyController : BaseController
    {
        private IFileUploader fileUploader;

        public CompanyController(IPickAndBookData data) 
            : base(data)
        {
        }

        public CompanyController(IPickAndBookData data, IFileUploader fileUploader)
            : base(data)
        {
            this.fileUploader = fileUploader;
        }

        public ActionResult Index()
        {
            Company currentCompany = this.Data.Companies.GetCompanyByUserId(User.Identity.GetUserId());
            if (currentCompany == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(currentCompany);
        }

        public ActionResult Edit(Guid? companyId)
        {
            Company currentCompany = this.Data.Companies.GetById(companyId);
            if (currentCompany == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CategoryId = new SelectList(this.Data.Categories.All().ToList(), "CategoryId", "CategoryName", currentCompany.CategoryId);
            return View(currentCompany);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company, HttpPostedFileBase upload)
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

                this.Data.Companies.Update(company);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(this.Data.Categories.All().ToList(), "CategoryId", "CategoryName", company.CategoryId);
            return View(company);
        }
    }
}