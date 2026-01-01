using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi; // <-- Updated namespace (no .Models)

using MultiVendor_WebApiServer.Models;


namespace MultiVendor_WebApiServer.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // lowercase "bearer" is recommended
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Enter your token only (Swagger adds 'Bearer ' automatically)."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                    // or = [] for C# 12 collection expression
                });
            });

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
