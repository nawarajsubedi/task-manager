using System.Web.Http;
using DotNetAssignment.IOC;
using Microsoft.Owin.Security.OAuth;

namespace DotNetAssignment
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = IoCContainer.RegisterUnityContainer();
            config.DependencyResolver = new UnityResolver(container);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
