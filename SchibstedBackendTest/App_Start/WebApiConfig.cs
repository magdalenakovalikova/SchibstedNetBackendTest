using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using SchibstedBackendTest.EFModels;
using SchibstedBackendTest.EFModels.Models;
using SchibstedBackendTest.Filters;
using SchibstedBackendTest.Services;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;

namespace SchibstedBackendTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new ApiAuthenticationFilter());
            config.Filters.Add(new IdentityBasicAuthenticationAttribute());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            container.RegisterType<IUserServices, UserServices>();
            container.RegisterType<DbContext, ApplicationDbContext>(
new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new HierarchicalLifetimeManager());

            UnityResolver unityResolver = new UnityResolver(container);
            config.DependencyResolver = unityResolver;
            DependencyResolver.SetResolver(unityResolver);
        }
    }
}