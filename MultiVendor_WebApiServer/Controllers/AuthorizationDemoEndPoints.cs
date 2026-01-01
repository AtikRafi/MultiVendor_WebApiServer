
using Microsoft.AspNetCore.Authorization;

namespace MultiVendor_WebApiServer.Controllers
{
    public static class AuthorizationDemoEndPoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/AdminOnly", AdminOnly);

            app.MapGet("/AdminOrTeacher",[Authorize(Roles ="StoreOwner")] () =>{
                return "Admin or Teacher";
            });

            //app.MapGet("/LiberaryMembersOnly", [Authorize(Policy = "HasLibrary")] () => {
            //    return "Library members only";
            //});

            //app.MapGet("/LiberaryMembersOnly", [Authorize(Roles= "Teacher", Policy = "HasLibrary")] () => {
            //    return "Library members only";
            //});

            //app.MapGet("/LiberaryMembersOnly", [Authorize(Policy = "HasLibrary")] [Authorize(Policy = "HasLibrary")] () => {
            //    return "Library members only";
            //});
            return app;
        }

        [Authorize(Roles ="Admin")]
        public static string AdminOnly()
        {
            return "admin Only";
        }
    }
}
