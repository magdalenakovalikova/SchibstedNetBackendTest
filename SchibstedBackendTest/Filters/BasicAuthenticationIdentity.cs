using System.Security.Principal;

namespace SchibstedBackendTest.Filters
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public string Password { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        public BasicAuthenticationIdentity(string userName, string password)
            : base(userName, "Basic")
        {
            Password = password;
            UserName = userName;
        }
    }
}