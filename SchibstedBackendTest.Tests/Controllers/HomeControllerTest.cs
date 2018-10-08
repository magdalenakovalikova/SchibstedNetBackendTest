using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchibstedBackendTest.Controllers;
using System.Web.Mvc;

namespace SchibstedBackendTest.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("SchibstedBackendTest - Home Page", result.ViewBag.Title);
        }
    }
}