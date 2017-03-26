using Moq;
using NUnit.Framework;
using PickAndBook.Data;
using PickAndBook.Helpers;
using PickAndBook.Helpers.Contracts;
using PickAndBook.Tests.Helpers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace PickAndBook.Tests.Controllers.CategoriesControllerTests
{
    [TestFixture]
    public class UploadFile_Should
    {
        [Test]
        public void ReturnEmptyString_WhenImageNotUploaded()
        {
            // Arrange
            var postedFileMock = new Mock<HttpPostedFileBase>();
            using (var stream = new MemoryStream())
            using (var bmp = new Bitmap(1, 1))
            {
                var graphics = Graphics.FromImage(bmp);
                graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
                bmp.Save(stream, ImageFormat.Jpeg);

                postedFileMock.Setup(pf => pf.InputStream).Returns(stream);
                postedFileMock.Setup(pf => pf.ContentLength).Returns((int)stream.Length);
                postedFileMock.Setup(pf => pf.FileName).Returns("TestImage");
            }
            postedFileMock.Setup(pf => pf.SaveAs(It.IsAny<String>()))
            .Throws(new Exception());

            IFileUploader fileUploader = new FileUploader(new TestPathProvider());
            string path = "/Test";

            // Act
            string uploadedImage = fileUploader.UploadFile(postedFileMock.Object, path);

            // Assert
            Assert.IsEmpty(uploadedImage);
        }

        [Test]
        public void ReturnExpectedImageName_WhenImageIsUploadedSuccesfully()
        {
            // Arrange
            var postedFileMock = new Mock<HttpPostedFileBase>();
            using (var stream = new MemoryStream())
            using (var bmp = new Bitmap(1, 1))
            {
                var graphics = Graphics.FromImage(bmp);
                graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
                bmp.Save(stream, ImageFormat.Jpeg);

                postedFileMock.Setup(pf => pf.InputStream).Returns(stream);
                postedFileMock.Setup(pf => pf.ContentLength).Returns((int)stream.Length);
                postedFileMock.Setup(pf => pf.FileName).Returns("TestImage");
            }

            var dataMock = new Mock<IPickAndBookData>();
            IFileUploader fileUploader = new FileUploader(new TestPathProvider());
            string path = "/Test";
            string expectedImage = Path.Combine(path, "TestImage");

            // Act
            string uploadedImage = fileUploader.UploadFile(postedFileMock.Object, path);

            // Assert
            Assert.AreEqual(expectedImage, uploadedImage);
        }
    }
}
