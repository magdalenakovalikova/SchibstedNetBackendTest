using Microsoft.AspNet.Identity;
using SchibstedBackendTest.Models;

namespace SchibstedBackendTest.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationUserManager _userManager;

        /// <summary>
        /// Public constructor.
        /// </summary>
        //public UserServices(/*SchibstedNetBackendTestDbContext*/ApplicationDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //    //_userManager = new ApplicationUserManager()
        //}
        public UserServices(/*SchibstedNetBackendTestDbContext*/ApplicationUserManager userManager)
        {
            _userManager = userManager;
            //_userManager = new ApplicationUserManager()
        }

        /// <summary>
        /// Public method to authenticate user by user name and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Authenticate(string username, string password)
        {
            var user = _userManager.FindByName(username);
            if (user != null)
            {
                PasswordVerificationResult result = _userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user.Id;
                    //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    //identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
                    //identity.AddClaim(new Claim(ClaimTypes.Email, user.UserName));
                    //context.Validated(identity);
                }
            }
            return null;
        }
    }

    public interface IUserServices
    {
        string Authenticate(string username, string password);
    }
}