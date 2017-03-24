using PickAndBook.Controllers;
using PickAndBook.Data.Common;
using System.Web.Mvc;
using PickAndBook.Data;

namespace PickAndBook.Areas.Admin.Controllers.Base
{
    [Authorize(Roles = DataConstants.AdministratorRoleName)]
    public abstract class AdminController : BaseController
    {
        public AdminController(IPickAndBookData data) 
            : base(data)
        {
        }
    }
}