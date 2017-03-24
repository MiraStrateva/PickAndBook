using Bytes2you.Validation;
using PickAndBook.Data;
using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PickAndBook.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IPickAndBookData data)
        {
            Guard.WhenArgument(data, nameof(data)).IsNull().Throw();

            this.Data = data;
        }

        protected IPickAndBookData Data { get; set; }

        protected internal RedirectToRouteResult RedirectToAction<TController>(Expression<Action<TController>> expression)
                where TController : Controller
        {
            var method = expression.Body as MethodCallExpression;
            if (method == null)
            {
                throw new ArgumentException("Expected method call");
            }

            return this.RedirectToAction(method.Method.Name);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (this.Request.IsAjaxRequest())
            {
                var exception = filterContext.Exception as HttpException;

                if (exception != null)
                {
                    this.Response.StatusCode = exception.GetHttpCode();
                    this.Response.StatusDescription = exception.Message;
                }
            }
            else
            {
                var controllerName = this.ControllerContext.RouteData.Values["Controller"].ToString();
                var actionName = this.ControllerContext.RouteData.Values["Action"].ToString();
                this.View("Error", new HandleErrorInfo(filterContext.Exception, controllerName, actionName)).ExecuteResult(this.ControllerContext);
            }

            filterContext.ExceptionHandled = true;
        }
    }
}