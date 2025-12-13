using Microsoft.EntityFrameworkCore;

namespace MultiVendor_WebApiServer.Models
{

    // ----------------------------
    // DbContext
    // ----------------------------
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<VendorOwner> VendorOwners { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProperty> CategoryProperties { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductVariantImage> ProductVariantImages { get; set; }
        public DbSet<ProductPropertyValue> ProductPropertyValues { get; set; }
        public DbSet<ProductVariantPropertyValue> ProductVariantPropertyValues { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -----------------------
            // Unique Indexes
            // -----------------------
            modelBuilder.Entity<VendorOwner>().HasIndex(vo => vo.NIDNumber).IsUnique();
            modelBuilder.Entity<Vendor>().HasIndex(v => v.VendorSlug).IsUnique();

            // -----------------------
            // Relationships with Cascade where safe
            // -----------------------
            modelBuilder.Entity<VendorOwner>()
                .HasMany(vo => vo.Vendors)
                .WithOne(v => v.VendorOwner)
                .HasForeignKey(v => v.VendorOwnerId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Products)
                .WithOne(p => p.Vendor)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Properties)
                .WithOne(cp => cp.Category)
                .HasForeignKey(cp => cp.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            modelBuilder.Entity<Product>()
                .HasMany(p => p.PropertyValues)
                .WithOne(ppv => ppv.Product)
                .HasForeignKey(ppv => ppv.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            modelBuilder.Entity<ProductVariant>()
                .HasMany(v => v.Images)
                .WithOne(vi => vi.Variant)
                .HasForeignKey(vi => vi.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            modelBuilder.Entity<ProductVariant>()
                .HasMany(v => v.Properties)
                .WithOne(pvpv => pvpv.Variant)
                .HasForeignKey(pvpv => pvpv.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade); // Safe

            // -----------------------
            // CartItems & Orders: Restrict (No Cascade)
            // -----------------------
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
        }
    }
}
