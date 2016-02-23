using System;
using System.Globalization;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;

namespace nugetory.Endpoint
{
    internal class OwinHost
    {
        private static IDisposable Server { get; set; }
        private static string ApiKey { get; set; }

        public static void Start(int port, string apiKey)
        {
            ApiKey = apiKey;

            string baseAddress = "http://+:" + port + "/";

            Server = WebApp.Start<OwinHost>(baseAddress);
        }

        public static void Stop()
        {
            Server.Dispose();
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureRoutes(config.Routes);

            ConfigureJson(config.Formatters.JsonFormatter);

            ConfigureFilters(config.Filters, ApiKey);

            appBuilder.UseWebApi(config);
        }

        private static void ConfigureRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute("ApiV2Root", "api/v2", new {controller = "root"});

            routes.MapHttpRoute("ApiV2Package", "api/v2/package", new {controller = "package"});

            routes.MapHttpRoute("ApiV2Packages", "api/v2/Packages(Id='{id}',Version='{version}')",
                                new {controller = "packageDetails"});

            routes.MapHttpRoute("ApiV2PackageDetails", "api/v2/package/{id}/{version}",
                                new {controller = "packageDownload"});

            routes.MapHttpRoute("ApiV2FindPackagesById", "api/v2/FindPackagesById()",
                                new {controller = "findPackage"});

            routes.MapHttpRoute("ApiV2PackageDelete", "api/v2/{id}/{version}",
                                new {controller = "packageDetails"});
        }

        private static void ConfigureJson(BaseJsonMediaTypeFormatter jsonFormatter)
        {
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            jsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            jsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonFormatter.SerializerSettings.Culture = CultureInfo.InvariantCulture;
        }

        private static void ConfigureFilters(HttpFilterCollection filters, string apiKey)
        {
            filters.Add(new ExceptionFilter());
            if (!string.IsNullOrEmpty(apiKey))
            {
                ValidateAuthenticationAttribute.ApiKey = apiKey;
                filters.Add(new ValidateAuthenticationAttribute());
            }
        }
    }
}
