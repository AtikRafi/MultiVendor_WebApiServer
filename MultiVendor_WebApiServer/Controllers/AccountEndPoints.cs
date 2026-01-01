using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MultiVendor_WebApiServer.Models;
using System.Security.Claims;

namespace MultiVendor_WebApiServer.Controllers
{
    public static class AccountEndPoints
    {
        public static IEndpointRouteBuilder MapAccountEndPoints( this IEndpointRouteBuilder app)
        {
            app.MapGet("/UserProfile", GetUserProfile);
            return app;
        }


        [Authorize]
        private static async Task<IResult> GetUserProfile(ClaimsPrincipal user, UserManager<ApplicantUser> userManager)
        {
            var userID = user.Claims.First(x=> x.Type== "UserId").Value;
            var details = await userManager.FindByIdAsync(userID);
            return Results.Ok(
                new
                {
                    Email = details?.Email,
                    FullName = details?.FullName
                }
            );
        }
    }
}
