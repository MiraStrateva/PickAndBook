using PickAndBook.Areas.Admin.Controllers.Base;
using PickAndBook.Common;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Helpers.Contracts;
using PickAndBook.Models.Shared;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PickAndBook.Areas.Admin.Controllers
{
    public class CategoriesController : AdminController
    {
        private readonly IFileUploader fileUploader;
        public CategoriesController(IPickAndBookData data)
            : base(data)
        {
        }

        public CategoriesController(IPickAndBookData data, IFileUploader fileProvider)
            : base(data)
        {
            this.fileUploader = fileProvider;
        }

        [HttpGet]
        public ActionResult Index(int page = 0, int pageSize = Constants.DefaultPageSize)
        {
            var categories = this.Data.Categories.GetAll(page, pageSize)
                       .ToList();
            var viewModel = new PageableViewModel<Category>()
            {
                Items = categories,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = this.Data.Categories.All().Count()
            };

            if (this.Request != null && this.Request.IsAjaxRequest())
            {
                return this.PartialView("_CategoryList", viewModel);
            }

            return this.View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(Guid? categoryId)
        {
            Category category = this.Data.Categories.GetById(categoryId);
            return this.View(category);
        }

        [HttpGet]
        public ActionResult Edit(Guid? categoryId)
        {
            Category category = this.Data.Categories.GetById(categoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string uploadedImage = this.fileUploader.UploadFile(upload, Constants.CategoriesImageFolder);
                    if (uploadedImage == "")
                    {
                        ModelState.AddModelError(Constants.FileCannotBeUploadedKey, Constants.FileCannotBeUploaded);
                        return View(category);
                    }

                    category.CategoryImage = uploadedImage;
                }
                this.Data.Categories.Update(category);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Delete(Guid? categoryId)
        {
            Category category = this.Data.Categories.GetById(categoryId);
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(Guid? categoryId)
        {
            Category category = this.Data.Categories.GetById(categoryId);
            this.Data.Categories.Delete(category);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category category, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string uploadedImage = this.fileUploader.UploadFile(upload, Constants.CategoriesImageFolder);
                    if (uploadedImage == "")
                    {
                        ModelState.AddModelError(Constants.FileCannotBeUploadedKey, Constants.FileCannotBeUploaded);
                        return View(category);
                    }
                    category.CategoryImage = uploadedImage;
                }

                this.Data.Categories.Add(category);
                this.Data.SaveChanges();
                return this.RedirectToAction("Details", new { categoryId = category.CategoryId });
            }

            return View(category);
        }
    }
}