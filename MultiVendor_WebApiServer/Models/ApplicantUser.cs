using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiVendor_WebApiServer.Models
{
    // ----------------------------
    // Identity User
    // ----------------------------
    public class ApplicantUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; } = null!;

        //[PersonalData]
        //[Column(TypeName = "nvarchar(10)")]
        //public string Gender { get; set; } = null!;

        //[PersonalData]
        //public DateOnly DOB { get; set; }
        public VendorOwner? VendorOwner { get; set; }

      

    }

}
