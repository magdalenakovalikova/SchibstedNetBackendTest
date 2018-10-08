using Microsoft.AspNet.Identity;

namespace SchibstedBackendTest.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationUserManager _userManager;

        public UserServices(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public string Authenticate(string username, string password)
        {
            var user = _userManager.FindByName(username);
            if (user != null)
            {
                PasswordVerificationResult result = _userManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user.Id;
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