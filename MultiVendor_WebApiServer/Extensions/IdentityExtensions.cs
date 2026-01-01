using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
                .AddRoles<IdentityRole>()
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
            services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["appSetting:jwtSecret"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew= TimeSpan.Zero
                };
            });

            services.AddAuthorization(option =>
            {
                option.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

                //option.AddPolicy("HasLibraryId", policy => policy.RequireClaim("LibraryID"));
                //option.AddPolicy("FemalesOnly", policy => policy.RequireClaim("Gender","Female"));
                //option.AddPolicy("Under10", policy => policy.RequireAssertion(context=>
                //    Int32.Parse(context.User.Claims.First(x=> x.Type=="Age").Value)<10));

            });
            return services;

        }

        public static WebApplication AddIdentityAuthMiddleWares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            
            return app;
        }
    }
}
