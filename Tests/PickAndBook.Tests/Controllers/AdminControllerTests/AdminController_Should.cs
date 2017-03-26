using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers.Base;
using PickAndBook.Tests.Helpers;
using System;

namespace PickAndBook.Tests.Controllers.AdminControllerTests
{
    [TestFixture]
    public class AdminController_Should
    {
        [TestCase(typeof(UnauthorizedAccessException))]
        public void HasAdminAuthorizationAttribute_Always(Type attrType)
        {
            // Arrange & Act & Assert
            AttributeTester.EnsureClassHasAdminAuthorizationAttribute(typeof(AdminController));
        }
    }
}
