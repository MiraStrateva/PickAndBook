using Moq;
using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers;
using PickAndBook.Areas.Admin.Controllers.Base;
using PickAndBook.Data;
using PickAndBook.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickAndBook.Tests.Controllers.AdminControllerTests
{
    [TestFixture]
    public class AdminController_Should
    {
        [TestCase(typeof(UnauthorizedAccessException))]
        public void HasAuthorizeAttribute_Always(Type attrType)
        {
            // Arrange & Act & Assert
            AttributeTester.EnsureClassHasAdminAuthorizationAttribute(typeof(AdminController));
        }
    }
}
