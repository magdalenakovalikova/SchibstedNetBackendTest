using SchibstedBackendTest.EFModels;
using SchibstedBackendTest.Filters;
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
    //[Authorize]
    //[RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [IdentityBasicAuthentication]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/users
        //[IdentityBasicAuthentication]
        [Route("api/Users")]
        public HttpResponseMessage GetUsers()
        {
            var db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var user in db.Users.Include("Roles"))
            {
                users.Add(new UserViewModel()
                {
                    username = user.UserName,
                    roles = user.Roles.Select(r => r.RoleId).ToArray()
                });
            }
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        private class UserViewModel
        {
            public UserViewModel()
            {
            }

            public string username { get; set; }
            public IEnumerable<string> roles { get; set; }
        }
    }
}