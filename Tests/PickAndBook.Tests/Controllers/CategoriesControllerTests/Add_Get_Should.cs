using Moq;
using NUnit.Framework;
using PickAndBook.Areas.Admin.Controllers;
using PickAndBook.Data;
using TestStack.FluentMVCTesting;

namespace PickAndBook.Tests.Controllers.CategoriesControllerTests
{
    [TestFixture]
    public class Add_Get_Should
    {
        [Test]
        public void ReturnDefaultViewWithExpectedModelType_WhenGetToAdd()
        {
            // Arrange
            var dataMock = new Mock<IPickAndBookData>();
            CategoriesController controller = new CategoriesController(dataMock.Object);

            // Act && Assert
            controller.WithCallTo(c => c.Add())
                .ShouldRenderDefaultView();
        }
    }
}
