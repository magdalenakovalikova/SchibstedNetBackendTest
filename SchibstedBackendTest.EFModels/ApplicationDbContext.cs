using Microsoft.AspNet.Identity.EntityFramework;
using SchibstedBackendTest.EFModels.Models;

namespace SchibstedBackendTest.EFModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SchibstedBackendTestContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}