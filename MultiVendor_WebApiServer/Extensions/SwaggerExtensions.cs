using MultiVendor_WebApiServer.Models;

namespace MultiVendor_WebApiServer.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
        {
            services.AddOpenApi();
            services.AddSwaggerGen();


            return services;
        }

        public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            return app;
        }
    }
}
