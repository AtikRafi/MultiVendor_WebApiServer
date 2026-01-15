using Microsoft.AspNetCore.Identity;
using MultiVendor_WebApiServer.Models;

namespace MultiVendor_WebApiServer.Services
{
    public static class RuntimeSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicantUser>>();

            // 1️⃣ Create Identity user
            var user = await userManager.FindByEmailAsync("vendor@demo.com");
            if (user == null)
            {
                user = new ApplicantUser
                {
                    UserName = "vendor@demo.com",
                    Email = "vendor@demo.com",
                    FullName = "John Doe",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Vendor@123");
                await userManager.AddToRoleAsync(user, "VendorOwner");
            }

            // 2️⃣ Create VendorOwner
            var vendorOwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            if (!context.VendorOwners.Any(vo => vo.UserId == user.Id))
            {
                var vendorOwner = new VendorOwner
                {
                    Id = vendorOwnerId,
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "0123456789",
                    Address = "Dhaka",
                    NIDNumber = "1234567890",
                    UserId = user.Id
                };
                context.VendorOwners.Add(vendorOwner);
                await context.SaveChangesAsync();
            }

            // 3️⃣ Create Vendor
            var vendorId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            if (!context.Vendors.Any(v => v.Id == vendorId))
            {
                context.Vendors.Add(new Vendor
                {
                    Id = vendorId,
                    VendorName = "Pacific Store",
                    VendorSlug = "pacific-store",
                    VendorPhone = "01700000000",
                    VendorAddress = "Banani, Dhaka",
                    City = "Dhaka",
                    State = "Dhaka",
                    Country = "Bangladesh",
                    TradeLicenseNo = "TL-123456",
                    VendorOwnerId = vendorOwnerId
                });
                await context.SaveChangesAsync();
            }
        }
    }

}
