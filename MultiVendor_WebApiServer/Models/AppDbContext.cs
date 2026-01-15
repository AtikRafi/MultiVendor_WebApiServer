using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MultiVendor_WebApiServer.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<VendorOwner> VendorOwners { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProperty> CategoryProperties { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductPropertyValue> ProductPropertyValues { get; set; }
        public DbSet<ProductVariantPropertyValue> ProductVariantPropertyValues { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<ApplicantUser> ApplicantUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -----------------------
            // Unique Indexes
            // -----------------------
            modelBuilder.Entity<VendorOwner>().HasIndex(vo => vo.NIDNumber).IsUnique();
            modelBuilder.Entity<Vendor>().HasIndex(v => v.VendorSlug).IsUnique();

            // -----------------------
            // Relationships
            // -----------------------
            modelBuilder.Entity<VendorOwner>()
                .HasMany(vo => vo.Vendors)
                .WithOne(v => v.VendorOwner)
                .HasForeignKey(v => v.VendorOwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Products)
                .WithOne(p => p.Vendor)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.PropertyValues)
                .WithOne(ppv => ppv.Product)
                .HasForeignKey(ppv => ppv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariant>()
                .HasMany(v => v.Properties)
                .WithOne(pvpv => pvpv.Variant)
                .HasForeignKey(pvpv => pvpv.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Customer)
                .HasForeignKey(ci => ci.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.ProductVariant)
                .WithMany()
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Vendor)
                .WithMany()
                .HasForeignKey(ci => ci.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.ProductVariant)
                .WithMany()
                .HasForeignKey(od => od.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Vendor)
                .WithMany()
                .HasForeignKey(od => od.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Delivery)
                .WithOne(d => d.Order)
                .HasForeignKey<Delivery>(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------
            // Enum conversions
            // -----------------------
            modelBuilder.Entity<CategoryProperty>()
                .Property(cp => cp.DataType)
                .HasConversion<string>();

            modelBuilder.Entity<Delivery>()
                .Property(d => d.DeliveryStatus)
                .HasConversion<string>();

            // -----------------------
            // Safe seed values
            // -----------------------
            // Only seed Category (no FK to Identity)
            var categoryId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = categoryId,
                Name = "Electronics"
            });

            var productId = Guid.Parse("77777777-7777-7777-7777-777777777777");
            
            var vendorId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = productId,
                Name = "Galaxy S25",
                BrandName = "Samsung",
                Description = "Latest Samsung flagship smartphone with high performance",
                CategoryId = categoryId,
                VendorId = vendorId,
                IsPublished = true,
                CreatedAt = new DateTime(2026, 1, 4, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = null
            });


            // ❌ VendorOwner and Vendor will be seeded at runtime to avoid FK conflicts
        }
    }
}
