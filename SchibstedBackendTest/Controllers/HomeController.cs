using SchibstedBackendTest.Enums;
using SchibstedBackendTest.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SchibstedBackendTest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize(Roles = "ADMIN, PAGE_1, PAGE_2, PAGE_3")]
        public ActionResult Index()
        {
            ViewBag.Title = "SchibstedBackendTest - Home Page";

            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var user in db.Users.Include("Roles"))
            {
                users.Add(new UserViewModel()
                {
                    username = user.UserName,
                    //roles = user.Roles.Select(r => r.RoleId).ToArray(),
                    IsADMIN = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN),
                    IsPAGE_1 = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1),
                    IsPAGE_2 = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2),
                    IsPAGE_3 = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3),
                });
            }
            return View(users.OrderBy(u => u.username));
        }
        [Authorize(Roles = "PAGE_1")]
        public ActionResult Page1()
        {
            ViewBag.Title = "SchibstedBackendTest - Page 1";

            return View();
        }
        [Authorize(Roles = "PAGE_2")]
        public ActionResult Page2()
        {
            ViewBag.Title = "SchibstedBackendTest - Page 2";

            return View();
        }
        [Authorize(Roles = "PAGE_3")]
        public ActionResult Page3()
        {
            ViewBag.Title = "SchibstedBackendTest - Page 3";

            return View();
        }
    }
}