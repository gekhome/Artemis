using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Artemis.Infrastructure.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ConfigurationCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public ConfigurationCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.development.json", true, true)
                .Build();

            string culture = Configuration.GetSection("Cultures").GetValue<string>("DefaultCulture")
                ?? throw new InvalidOperationException($"Culture 'DefaultCulture' not found in appsettings.json file!");

            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseConfigurationCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConfigurationCultureMiddleware>();
        }
    }
}
