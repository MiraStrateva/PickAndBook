using NUnit.Framework;
using System;
using PickAndBook.Controllers;
using PickAndBook.Tests.Helpers;
using System.Web.Mvc;

namespace PickAndBook.Tests.Controllers.CompanyAnonymousControllerTests
{
    [TestFixture]
    public class CompanyAnonymousController_Should
    {
        [TestCase(typeof(AllowAnonymousAttribute))]
        public void HasAllowAnonymousAttribute_Always(Type attrType)
        {
            // Arrange & Act 
            var llResult = AttributeTester.ClassHasAttribute(typeof(CompanyAnonymousController), attrType);

            // Assert
            Assert.IsTrue(llResult);
        }
    }
}
