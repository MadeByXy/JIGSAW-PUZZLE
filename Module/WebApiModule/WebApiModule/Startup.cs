using Owin;
using System.Web.Http;

namespace WebApiModule
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Use the extension method provided by the WebApi.Owin library:
            app.UseWebApi(ConfigureWebApi);
        }

        private HttpConfiguration ConfigureWebApi
        {
            get
            {
                var config = new HttpConfiguration();
                config.Routes.MapHttpRoute(
                    "DefaultApi",
                    "Api/{controller}/{id}",
                    new { id = RouteParameter.Optional });
                return config;
            }
        }
    }
}
