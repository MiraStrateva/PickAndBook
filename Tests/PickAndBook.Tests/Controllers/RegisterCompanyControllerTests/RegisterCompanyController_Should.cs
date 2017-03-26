using NUnit.Framework;
using PickAndBook.Controllers;
using PickAndBook.Tests.Helpers;
using System;

namespace PickAndBook.Tests.Controllers.RegisterCompanyControllerTests
{
    public class RegisterCompanyController_Should
    {
        [TestCase(typeof(UnauthorizedAccessException))]
        public void HasClientAuthorizationAttribute_Always(Type attrType)
        {
            // Arrange & Act & Assert
            AttributeTester.EnsureClassHasClientAuthorizationAttribute(typeof(RegisterCompanyController));
        }
    }
}
