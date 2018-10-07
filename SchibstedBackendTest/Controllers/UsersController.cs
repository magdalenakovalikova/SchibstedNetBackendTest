using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchibstedBackendTest.Enums;
using SchibstedBackendTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchibstedBackendTest.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/values
        //[IdentityBasicAuthentication]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/users
        public HttpResponseMessage Get()
        {
            var username = User.Identity.Name;
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var user in db.Users.Include("Roles"))
            {
                users.Add(new UserViewModel()
                {
                    username = user.UserName,
                    IsADMIN = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN),
                    IsPAGE_1 = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1),
                    IsPAGE_2 = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2),
                    IsPAGE_3 = user.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3),
                });
            }
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        // GET api/values/5
        public HttpResponseMessage Get(string id)
        {
            var username = User.Identity.Name;
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            var dbUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == id);
            List<UserViewModel> users = new List<UserViewModel>();
            UserViewModel user = new UserViewModel()
            {
                username = dbUser.UserName,
                IsADMIN = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN),
                IsPAGE_1 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1),
                IsPAGE_2 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2),
                IsPAGE_3 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3),
            };
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]UserPasswordViewModel user)
        {
            var username = User.Identity.Name;
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (!db.Users.Any(u => u.UserName == user.username))
            {
                var appUser = new ApplicationUser { UserName = user.username };

                IdentityResult result = userManager.Create(appUser, user.password);

                if (!result.Succeeded)
                {
                    return Request.CreateResponse(GetErrorResult(result));
                }
                if (user.IsADMIN) userManager.AddToRole(appUser.Id, "ADMIN");
                if (user.IsPAGE_1) userManager.AddToRole(appUser.Id, "PAGE_1");
                if (user.IsPAGE_2) userManager.AddToRole(appUser.Id, "PAGE_2");
                if (user.IsPAGE_3) userManager.AddToRole(appUser.Id, "PAGE_3");
            }

            return Request.CreateResponse(HttpStatusCode.OK);//, user);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}