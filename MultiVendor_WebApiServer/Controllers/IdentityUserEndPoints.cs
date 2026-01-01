using Microsoft.AspNetCore.Authorization;
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
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public int Age { get; set; }
       
    }
    public class LogInModel
    {

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public static class IdentityUserEndPoints
    {
       
        public static IEndpointRouteBuilder MapIdentityUserEndPoints(this IEndpointRouteBuilder app)
        {

            app.MapPost("/signUp", CreateUser);
            

            app.MapPost("/signIn", SignIn);
            return app;
        }
        [AllowAnonymous]
        private static async Task<IResult> CreateUser(UserManager<ApplicantUser> userManager, [FromBody] UserRegistrationModel userRegistrationModel)
        {
            ApplicantUser user = new ApplicantUser()
            {
                UserName = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
                Gender = userRegistrationModel.Gender,
                DOB = DateOnly.FromDateTime(DateTime.Now.AddYears(-userRegistrationModel.Age)),
                Email = userRegistrationModel.Email,
            };

            var result = await userManager.CreateAsync( user,  userRegistrationModel.Password);

            await userManager.AddToRoleAsync(user, userRegistrationModel.Role);

            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);
        }
        [AllowAnonymous]
        private static async Task<IResult> SignIn(UserManager<ApplicantUser> userManager, [FromBody] LogInModel login,IOptions<appSetting> appSetting)
        {
            var user = await userManager.FindByEmailAsync(login.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, login.Password))
            {
                var roles = await userManager.GetRolesAsync(user);

                ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Gender", user.Gender.ToString()),
                    new Claim("Age", (DateTime.Now.Year - user.DOB.Year).ToString()),
                    new Claim(ClaimTypes.Role, roles.First())
                });

                //if(user.LibertyID != null)
                //{
                //    claims.AddClaims(new Claim("LibertyID", user.libertyID)!);
                //}

                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.Value.jwtSecret));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(1),
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
