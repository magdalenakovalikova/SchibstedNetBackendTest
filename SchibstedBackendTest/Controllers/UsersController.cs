using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchibstedBackendTest.Enums;
using SchibstedBackendTest.Models;
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
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (NotAuthorized(db))
                new HttpResponseMessage(HttpStatusCode.Unauthorized);

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
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (NotAuthorized(db))
                new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var dbUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == id);
            if (dbUser == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("User not found."));
            }
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

        private bool NotAuthorized(ApplicationDbContext db)
        {
            var username = User.Identity.Name;
            var dbUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == username);
            return (dbUser == null || !dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN));
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]UserPasswordViewModel user)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (NotAuthorized(db))
                new HttpResponseMessage(HttpStatusCode.Unauthorized);

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
        public HttpResponseMessage Put(string id, [FromBody]UserPasswordViewModel user)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (NotAuthorized(db))
                new HttpResponseMessage(HttpStatusCode.Unauthorized);

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store);

            var appUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == id);
            if (appUser != null)
            {
                if (appUser.UserName != user.username)
                {
                    appUser.UserName = user.username;
                }
                if (user.password == null || user.password.Length < 6)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid password format");
                }
                PasswordVerificationResult verificationResult = userManager.PasswordHasher.VerifyHashedPassword(appUser.PasswordHash, user.password);

                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    string hashedNewPassword = userManager.PasswordHasher.HashPassword(user.password);
                    store.SetPasswordHashAsync(appUser, hashedNewPassword);
                }

                if (user.IsADMIN && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN)) userManager.AddToRole(appUser.Id, "ADMIN");
                else if (!user.IsADMIN && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN)) userManager.RemoveFromRole(appUser.Id, "ADMIN");
                if (user.IsPAGE_1 && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1)) userManager.AddToRole(appUser.Id, "PAGE_1");
                else if (!user.IsPAGE_1 && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1)) userManager.RemoveFromRole(appUser.Id, "PAGE_1");
                if (user.IsPAGE_2 && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2)) userManager.AddToRole(appUser.Id, "PAGE_2");
                else if (!user.IsPAGE_2 && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2)) userManager.RemoveFromRole(appUser.Id, "PAGE_2");
                if (user.IsPAGE_3 && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3)) userManager.AddToRole(appUser.Id, "PAGE_3");
                else if (!user.IsPAGE_3 && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3)) userManager.RemoveFromRole(appUser.Id, "PAGE_3");

                store.UpdateAsync(appUser);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(string id)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (NotAuthorized(db))
                new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var dbUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == id);
            if (dbUser == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("User not found."));
            }
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            userManager.Delete(dbUser);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}