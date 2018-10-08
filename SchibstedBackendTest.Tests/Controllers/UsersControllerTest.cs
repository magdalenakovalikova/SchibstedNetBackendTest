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
    }
}