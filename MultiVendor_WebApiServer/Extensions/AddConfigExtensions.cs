using MultiVendor_WebApiServer.Models;

namespace MultiVendor_WebApiServer.Extensions
{
    public static class AddConfigExtensions
    {
        public static WebApplication ConfigureCros(this WebApplication app,IConfiguration configuration)
        {
            app.UseCors(options =>
                    options.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
          

            return app;
        }
        public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<appSetting>(configuration.GetSection("appSetting"));
            return services;
        }
    }
}
