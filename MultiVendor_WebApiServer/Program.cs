using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiVendor_WebApiServer.Controllers;
using MultiVendor_WebApiServer.Extensions;
using MultiVendor_WebApiServer.Models;
using MultiVendor_WebApiServer.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerExplorer()
                .InjectDbContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);





builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


//authentication service add


var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCros(builder.Configuration)
    .AddIdentityAuthMiddleWares();

app.UseHttpsRedirection();



app.MapControllers();

app.MapGroup("/api")
   .MapIdentityApi<ApplicantUser>();
app.MapGroup("/api")
   .MapIdentityUserEndPoints();





app.Run();


