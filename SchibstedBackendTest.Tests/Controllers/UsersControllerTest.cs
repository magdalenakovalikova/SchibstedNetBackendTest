using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchibstedBackendTest.Controllers;
using SchibstedBackendTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SchibstedBackendTest.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(context.TestDeploymentDir, string.Empty));
        }

        [TestMethod]
        public void Get()
        {
            // Arrange
            UsersController controller = new UsersController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            /*IEnumerable<string>*/
            HttpResponseMessage result = controller.Get();
            List<UserViewModel> users = null;
            if (result != null)
            {
                result.TryGetContentValue(out users);
            }

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Where(u => u.IsADMIN).Count() > 0);
            Assert.IsTrue(users.Where(u => u.IsPAGE_1).Count() > 0);
            Assert.IsTrue(users.Where(u => u.IsPAGE_2).Count() > 0);
            Assert.IsTrue(users.Where(u => u.IsPAGE_3).Count() > 0);
            //Assert.AreEqual("value1", users.ElementAt(0));
            //Assert.AreEqual("value2", users.ElementAt(1));
        }

        //[TestMethod]
        //public void GetById()
        //{
        //    // Arrange
        //    UsersController controller = new UsersController();
        //    controller.Request = new HttpRequestMessage();
        //    controller.Configuration = new HttpConfiguration();

        //    // Act
        //    HttpResponseMessage result = controller.Get("admin");

        //    // Assert
        //    Assert.AreEqual("value", result);
        //}

        //[TestMethod]
        //public void Post()
        //{
        //    // Arrange
        //    UsersController controller = new UsersController();

        //    // Act
        //    controller.Post(null);

        //    // Assert
        //}

        //[TestMethod]
        //public void Put()
        //{
        //    // Arrange
        //    UsersController controller = new UsersController();

        //    // Act
        //    controller.Put(5, "value");

        //    // Assert
        //}

        //[TestMethod]
        //public void Delete()
        //{
        //    // Arrange
        //    UsersController controller = new UsersController();

        //    // Act
        //    controller.Delete(5);

        //    // Assert
        //}
    }
}