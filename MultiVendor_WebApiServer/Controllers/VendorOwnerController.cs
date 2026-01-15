using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiVendor_WebApiServer.Models;
using MultiVendor_WebApiServer.Services;

namespace MultiVendor_WebApiServer.Controllers
{
    [Authorize(Roles = "VendorOwner")]
    [Route("api/[controller]")]
    [ApiController]
    public class VendorOwnerController : ControllerBase
    {
        private readonly AppDbContext _context;
        public VendorOwnerController(AppDbContext context)
        {
            _context= context;
        }
        [HttpGet("me/vendors")]
        public async Task<IActionResult> GetMyVendors()
        {
            //bcz i user 'this' in IDentityUserId
            var userID = User.GetUserId();

            var vendorOwner = await _context.VendorOwners.Include(vo => vo.Vendors).FirstOrDefaultAsync(v => v.UserId==userID);

            if (vendorOwner==null)
            {
                return NotFound("Vendor Owner Profile Not found");
            }

            var vendors = vendorOwner.Vendors.Select(v => new
            {
                v.Id,
                v.VendorName,
                v.VendorSlug,
                v.VendorLogoUrl

            }).ToList();
            return Ok(vendors);
        }
    }
}
