using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchibstedBackendTest.Controllers;
using SchibstedBackendTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
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
            UsersController controller = new UsersController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("admin", "Forms"), null);
            HttpResponseMessage result = controller.Get(FakeUser);
            List<UserViewModel> users = null;
            if (result != null)
            {
                result.TryGetContentValue(out users);
            }

            Assert.IsNotNull(result);
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Where(u => u.IsADMIN).Count() > 0);
            Assert.IsTrue(users.Where(u => u.IsPAGE_1).Count() > 0);
            Assert.IsTrue(users.Where(u => u.IsPAGE_2).Count() > 0);
            Assert.IsTrue(users.Where(u => u.IsPAGE_3).Count() > 0);
        }

        [TestMethod]
        public void Get_admin()
        {
            UsersController controller = new UsersController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("admin", "Forms"), null);
            HttpResponseMessage result = controller.Get("admin", FakeUser);
            UserViewModel user = null;
            if (result != null)
            {
                result.TryGetContentValue(out user);
            }

            Assert.IsNotNull(result);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.IsADMIN);
            Assert.IsFalse(user.IsPAGE_1);
            Assert.IsFalse(user.IsPAGE_2);
            Assert.IsFalse(user.IsPAGE_3);
        }

        [TestMethod]
        public void Get_nonExisting()
        {
            UsersController controller = new UsersController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("admin", "Forms"), null);
            string randomUsername = Guid.NewGuid().ToString();
            HttpResponseMessage result = controller.Get(randomUsername, FakeUser);
            UserViewModel user = null;
            if (result != null)
            {
                result.TryGetContentValue(out user);
            }

            Assert.IsNull(user);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void Create_get_and_Delete()
        {
            UsersController controller = new UsersController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("admin", "Forms"), null);
            UserPasswordViewModel testUser = new UserPasswordViewModel()
            {
                password = "string",
                username = "test",
                IsADMIN = true,
                IsPAGE_1 = true,
                IsPAGE_2 = true,
                IsPAGE_3 = true
            };
            //Act 1
            HttpResponseMessage result = controller.Post(testUser, FakeUser);
            //Assert 1
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK);
            //Act 2
            result = controller.Get("test", FakeUser);
            UserViewModel user = null;
            if (result != null)
            {
                result.TryGetContentValue(out user);
            }
            //Assert 2
            Assert.IsNotNull(result);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.IsADMIN, user.IsADMIN);
            Assert.AreEqual(user.IsPAGE_1, user.IsPAGE_1);
            Assert.AreEqual(user.IsPAGE_2, user.IsPAGE_2);
            Assert.AreEqual(user.IsPAGE_3, user.IsPAGE_2);
            //Act 3
            result = controller.Delete("test", FakeUser);
            //Assert 3
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK);
            result = controller.Get("test", FakeUser);
            user = null;
            if (result != null)
            {
                result.TryGetContentValue(out user);
            }

            Assert.IsNull(user);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.NotFound);
        }
    }
}