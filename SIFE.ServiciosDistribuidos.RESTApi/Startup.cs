using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using SIFE.ServiciosDistribuidos.RESTApi.Configuration;

[assembly: OwinStartup(typeof(SIFE.ServiciosDistribuidos.RESTApi.Startup))]

namespace SIFE.ServiciosDistribuidos.RESTApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var configuration = this.GetConfiguration();
            app.UseWebApi(configuration);
        }

        /// <summary>
        /// Gets the global web api configuration
        /// </summary>
        /// <returns>Global configuration</returns>
        public virtual System.Web.Http.HttpConfiguration GetConfiguration()
        {
            return ApiConfig.Register();
        }
    }
}
