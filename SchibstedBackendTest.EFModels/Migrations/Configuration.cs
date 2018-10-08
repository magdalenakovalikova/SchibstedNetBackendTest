namespace SchibstedBackendTest.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SchibstedBackendTest.EFModels;
    using SchibstedBackendTest.EFModels.Models;
    using SchibstedBackendTest.Enums;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityRole role;
            if (!context.Roles.Any(r => r.Name == "ADMIN"))
            {
                role = new IdentityRole { Id = AspNetRolesEnum.ADMIN, Name = "ADMIN" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "PAGE_1"))
            {
                role = new IdentityRole { Id = AspNetRolesEnum.PAGE_1, Name = "PAGE_1" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "PAGE_2"))
            {
                role = new IdentityRole { Id = AspNetRolesEnum.PAGE_2, Name = "PAGE_2" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "PAGE_3"))
            {
                role = new IdentityRole { Id = AspNetRolesEnum.PAGE_3, Name = "PAGE_3" };
                roleManager.Create(role);
            }

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user;
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                user = new ApplicationUser { UserName = "admin" };

                userManager.Create(user, "12345678");
                userManager.AddToRole(user.Id, "ADMIN");
            }
            if (!context.Users.Any(u => u.UserName == "user1"))
            {
                user = new ApplicationUser { UserName = "user1" };

                userManager.Create(user, "12345678");
                userManager.AddToRole(user.Id, "PAGE_1");
            }
            if (!context.Users.Any(u => u.UserName == "user2"))
            {
                user = new ApplicationUser { UserName = "user2" };

                userManager.Create(user, "12345678");
                userManager.AddToRole(user.Id, "PAGE_2");
            }
            if (!context.Users.Any(u => u.UserName == "user3"))
            {
                user = new ApplicationUser { UserName = "user3" };

                userManager.Create(user, "12345678");
                userManager.AddToRole(user.Id, "PAGE_3");
            }
        }
    }
}