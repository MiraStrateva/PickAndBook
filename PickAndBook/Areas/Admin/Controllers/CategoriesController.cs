using PickAndBook.Areas.Admin.Controllers.Base;
using PickAndBook.Common;
using PickAndBook.Data;
using PickAndBook.Data.Models;
using PickAndBook.Models.Shared;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PickAndBook.Areas.Admin.Controllers
{
    public class CategoriesController : AdminController
    {
        public CategoriesController(IPickAndBookData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Index(int page = 0, int pageSize = Constants.DefaultPageSize)
        {
            int toSkip = page * pageSize;
            var categories = this.Data.Categories.All()
                       .OrderBy(c => c.CategoryName)
                       .Skip(toSkip)
                       .Take(pageSize)
                       .ToList();
            var viewModel = new PageableViewModel<Category>()
            {
                Items = categories,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = this.Data.Categories.All().Count()
            };

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("_CategoryList", viewModel);
            }

            return this.View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(Guid? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = this.Data.Categories.GetById(categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }
            return this.View(category);
        }

        [HttpGet]
        public ActionResult Edit(Guid? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = this.Data.Categories.GetById(categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }
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
                    try
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/Categories"), fileName);
                        upload.SaveAs(path);

                        category.CategoryImage = Path.Combine("/Images/Categories/", fileName);
                    }
                    catch
                    {
                        ViewBag.Message = "File upload failed!!";
                        return View(category);
                    }
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
        public ActionResult DeleteConfirmed(Guid? categoryId)
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category category, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    try
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        string path = Path.Combine(Server.MapPath("~/Images/Categories"), fileName);
                        upload.SaveAs(path);
                        category.CategoryImage = Path.Combine("/Images/Categories/", fileName);
                    }
                    catch
                    {
                        ViewBag.Message = "File upload failed!!";
                        return View();
                    }
                }

                this.Data.Categories.Add(category);
                this.Data.SaveChanges();
                return this.RedirectToAction("Details", new { categoryId = category.CategoryId });
            }

            // If we got this far, something failed, redisplay form
            return View(category);
        }
    }
}