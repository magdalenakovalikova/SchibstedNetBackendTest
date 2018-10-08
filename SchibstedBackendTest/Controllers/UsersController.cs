using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchibstedBackendTest.EFModels;
using SchibstedBackendTest.EFModels.Models;
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
        [Route("api/users/{id}")]
        public HttpResponseMessage Delete(string id)
        {
            return Delete(id, User);
        }

        public HttpResponseMessage Delete(string id, System.Security.Principal.IPrincipal user)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (IsNotAuthorized(db, user.Identity.Name))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var dbUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == id);
            if (dbUser == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("User not found."));
            }
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            userManager.Delete(dbUser);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/users")]
        public HttpResponseMessage Get()
        {
            return Get(User);
        }

        public HttpResponseMessage Get(System.Security.Principal.IPrincipal user)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (IsNotAuthorized(db, user.Identity.Name))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var dbUser in db.Users.Include("Roles"))
            {
                users.Add(new UserViewModel()
                {
                    username = dbUser.UserName,
                    IsADMIN = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN),
                    IsPAGE_1 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1),
                    IsPAGE_2 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2),
                    IsPAGE_3 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3),
                });
            }
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [Route("api/users/{id}")]
        public HttpResponseMessage Get(string id)
        {
            return Get(id, User);
        }

        public HttpResponseMessage Get(string id, System.Security.Principal.IPrincipal user)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (IsNotAuthorized(db, user.Identity.Name))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var dbUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == id);
            if (dbUser == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError("User not found."));
            }
            List<UserViewModel> users = new List<UserViewModel>();
            UserViewModel userViewModel = new UserViewModel()
            {
                username = dbUser.UserName,
                IsADMIN = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN),
                IsPAGE_1 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1),
                IsPAGE_2 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2),
                IsPAGE_3 = dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3),
            };
            return Request.CreateResponse(HttpStatusCode.OK, userViewModel);
        }

        [Route("api/users")]
        public HttpResponseMessage Post([FromBody]UserPasswordViewModel userViewModel)
        {
            return Post(userViewModel, User);
        }

        public HttpResponseMessage Post(UserPasswordViewModel userViewModel, System.Security.Principal.IPrincipal user)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (IsNotAuthorized(db, user.Identity.Name))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (!db.Users.Any(u => u.UserName == userViewModel.username))
            {
                var appUser = new ApplicationUser { UserName = userViewModel.username };

                IdentityResult result = userManager.Create(appUser, userViewModel.password);

                if (!result.Succeeded)
                {
                    return Request.CreateResponse(GetErrorResult(result));
                }
                if (userViewModel.IsADMIN) userManager.AddToRole(appUser.Id, "ADMIN");
                if (userViewModel.IsPAGE_1) userManager.AddToRole(appUser.Id, "PAGE_1");
                if (userViewModel.IsPAGE_2) userManager.AddToRole(appUser.Id, "PAGE_2");
                if (userViewModel.IsPAGE_3) userManager.AddToRole(appUser.Id, "PAGE_3");
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/users/{id}")]
        public HttpResponseMessage Put(string id, [FromBody]UserPasswordViewModel user)
        {
            return Put(id, user, User);
        }

        public HttpResponseMessage Put(string id, UserPasswordViewModel userViewModel, System.Security.Principal.IPrincipal user)
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            if (IsNotAuthorized(db, user.Identity.Name))
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store);

            var appUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == id);
            if (appUser != null)
            {
                if (appUser.UserName != userViewModel.username)
                {
                    appUser.UserName = userViewModel.username;
                }
                if (userViewModel.password == null || userViewModel.password.Length < 6)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid password format");
                }
                PasswordVerificationResult verificationResult = userManager.PasswordHasher.VerifyHashedPassword(appUser.PasswordHash, userViewModel.password);

                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    string hashedNewPassword = userManager.PasswordHasher.HashPassword(userViewModel.password);
                    store.SetPasswordHashAsync(appUser, hashedNewPassword);
                }

                if (userViewModel.IsADMIN && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN)) userManager.AddToRole(appUser.Id, "ADMIN");
                else if (!userViewModel.IsADMIN && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN)) userManager.RemoveFromRole(appUser.Id, "ADMIN");
                if (userViewModel.IsPAGE_1 && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1)) userManager.AddToRole(appUser.Id, "PAGE_1");
                else if (!userViewModel.IsPAGE_1 && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_1)) userManager.RemoveFromRole(appUser.Id, "PAGE_1");
                if (userViewModel.IsPAGE_2 && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2)) userManager.AddToRole(appUser.Id, "PAGE_2");
                else if (!userViewModel.IsPAGE_2 && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_2)) userManager.RemoveFromRole(appUser.Id, "PAGE_2");
                if (userViewModel.IsPAGE_3 && !appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3)) userManager.AddToRole(appUser.Id, "PAGE_3");
                else if (!userViewModel.IsPAGE_3 && appUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.PAGE_3)) userManager.RemoveFromRole(appUser.Id, "PAGE_3");

                store.UpdateAsync(appUser);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
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
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private bool IsNotAuthorized(ApplicationDbContext db, string username = null)
        {
            if (username == null)
            {
                username = User.Identity.Name;
            }
            var dbUser = db.Users.Include("Roles").SingleOrDefault(u => u.UserName == username);
            return (dbUser == null || !dbUser.Roles.Any(r => r.RoleId == AspNetRolesEnum.ADMIN));
        }
    }
}