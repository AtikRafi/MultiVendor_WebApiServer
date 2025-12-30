using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MultiVendor_WebApiServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiVendor_WebApiServer.Controllers
{
    public class UserRegistrationModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LogInModel
    {

        public string Email { get; set; }
        public string Password { get; set; }
    }
    public static class IdentityUserEndPoints
    {
       
        public static IEndpointRouteBuilder MapIdentityUserEndPoints(this IEndpointRouteBuilder app)
        {

            app.MapPost("/signUp", CreateUser);
            

            app.MapPost("/signIn", SignIn);
            return app;
        }
        private static async Task<IResult> CreateUser(UserManager<ApplicantUser> userManager, [FromBody] UserRegistrationModel userRegistrationModel)
        {
            ApplicantUser user = new ApplicantUser()
            {
                UserName = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
                Email = userRegistrationModel.Email,
            };

            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);
            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);
        }

        private static async Task<IResult> SignIn(UserManager<ApplicantUser> userManager, [FromBody] LogInModel login,IOptions<appSetting> appSetting)
        {
            var user = await userManager.FindByEmailAsync(login.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, login.Password))
            {
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.Value.jwtSecret));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                new Claim("UserId",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)

                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKEy = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityKEy);
                return Results.Ok(new { token });
            }
            else
                return Results.BadRequest(new { message = "Email or Password not matched" });
        }
    }
}
