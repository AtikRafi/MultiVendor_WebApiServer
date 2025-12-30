using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MultiVendor_WebApiServer.Migrations;
using MultiVendor_WebApiServer.Models;
using System.Text;
using ApplicantUser = MultiVendor_WebApiServer.Models.ApplicantUser;

namespace MultiVendor_WebApiServer.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<ApplicantUser>()
                .AddEntityFrameworkStores<AppDbContext>();

            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
            });
            return services;

        }

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =
                x.DefaultChallengeScheme =
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["appSetting:jwtSecret"]!))
                };
            });
            return services;

        }

        public static WebApplication AddIdentityAuthMiddleWares(this WebApplication app)
        {
            app.UseAuthorization();
            app.UseAuthentication();
            return app;
        }
    }
}
