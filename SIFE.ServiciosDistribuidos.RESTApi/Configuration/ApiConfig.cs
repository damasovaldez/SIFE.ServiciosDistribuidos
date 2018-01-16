using SIFE.ServiciosDistribuidos.Core.Services;
using System.Net.Http.Formatting;
using System.Web.Http;
using Unity;

namespace SIFE.ServiciosDistribuidos.RESTApi.Configuration
{
    public static class ApiConfig
    {
        /// <summary>
        /// Initializes API Configuration
        /// </summary>
        /// <returns>Web api configuration</returns>
        public static HttpConfiguration Register()
        {
            // Web API configuration and services
            var config = new HttpConfiguration();

            // Habilita la inclusión de errores ya que en azure no se despliegan los errores.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                       name: "DefaultApi",
                       routeTemplate: "api/{controller}/{id}",
                       defaults: new { id = RouteParameter.Optional });

            // Se formatea el json para que salga en forma de cadena
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.EnableCors();

            // Attaches the Web api IOC container
            var container = CreateContainer();
            config.DependencyResolver = new UnityResolver(container);

            return config;
        }

        /// <summary>
        /// Creates and configures an IOC container
        /// </summary>
        /// <returns>Configured container</returns>
        public static UnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            // Context
            //container.RegisterType<IContext, ContextDBEntities>(new TransientLifetimeManager());

            // Services
            container.RegisterType<IComprobanteService, ComprobanteService>(new Unity.Lifetime.TransientLifetimeManager());

            // Repositories

            return container;
        }
    }
}