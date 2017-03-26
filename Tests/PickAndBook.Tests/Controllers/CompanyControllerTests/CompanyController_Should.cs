using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Tests.Helpers;
using System;

namespace PickAndBook.Tests.Controllers.CompanyControllerTests
{
    [TestFixture]
    public class CompanyController_Should
    {
        [TestCase(typeof(UnauthorizedAccessException))]
        public void HasCompanyAuthorizationAttribute_Always(Type attrType)
        {
            // Arrange & Act & Assert
            AttributeTester.EnsureClassHasCompanyAuthorizationAttribute(typeof(CompanyController));
        }
    }
}
