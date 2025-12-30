using Microsoft.EntityFrameworkCore;
using MultiVendor_WebApiServer.Models;

namespace MultiVendor_WebApiServer.Extensions
{
    public static  class EFCoreExtensions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x=> x.UseSqlServer(configuration.GetConnectionString("dbcs")));
            return services;
        }
    }
}
