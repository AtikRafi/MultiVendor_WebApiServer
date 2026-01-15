using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiVendor_WebApiServer.Models
{
    // ----------------------------
    // Enums
    // ----------------------------
    public enum PropertyDataType
    {
        String=1,
        Number=2,
        Boolean=3,
        Date=4
    }

    public enum DeliveryStatusEnum
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }

   
    // ----------------------------
    // Vendor Owner
    // ----------------------------
    public class VendorOwner
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required, StringLength(20)]
        public string Phone { get; set; } = null!;

        [Required, StringLength(500)]
        public string Address { get; set; } = null!;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required, StringLength(50)]
        public string NIDNumber { get; set; } = null!;
        [Required]
        public string UserId { get; set; } = null!;

        public ApplicantUser User { get; set; } = null!;

        // Navigation
        public ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
    }

    // ----------------------------
    // Vendor
    // ----------------------------
    public class Vendor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string VendorName { get; set; } = null!;

        [Required, StringLength(200)]
        public string VendorSlug { get; set; } = null!;

        [StringLength(1000)]
        public string? VendorDescription { get; set; }

        [StringLength(500)]
        public string? VendorLogoUrl { get; set; }

        [StringLength(500)]
        public string? VendorBannerUrl { get; set; }

        [Required, StringLength(20)]
        public string VendorPhone { get; set; } = null!;

        [Required, StringLength(500)]
        public string VendorAddress { get; set; } = null!;

        [Required, StringLength(100)]
        public string City { get; set; } = null!;

        [Required, StringLength(100)]
        public string State { get; set; } = null!;

        [Required, StringLength(100)]
        public string Country { get; set; } = null!;

        [Required, StringLength(100)]
        public string TradeLicenseNo { get; set; } = null!;

        [Required]
        public Guid VendorOwnerId { get; set; }

        [ForeignKey(nameof(VendorOwnerId))]
        public VendorOwner VendorOwner { get; set; } = null!;

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    // ----------------------------
    // Category
    // ----------------------------
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<CategoryProperty> Properties { get; set; } = new List<CategoryProperty>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class CategoryProperty
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public PropertyDataType DataType { get; set; }

        public bool IsVariant { get; set; } = false;

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }

    // ----------------------------
    // Product
    // ----------------------------
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? BrandName { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        public bool IsPublished { get; set; } = false;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;


        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;

        public ICollection<ProductPropertyValue> PropertyValues { get; set; } = new List<ProductPropertyValue>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

   

    // ----------------------------
    // Product Variant
    // ----------------------------
    public class ProductVariant
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductId { get; set; }

        [StringLength(100)]
        public string? Sku { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CompareAtPrice { get; set; }

        public int Stock { get; set; }
        public bool IsDefault { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Required, StringLength(500)]
        public string PicturePath { get; set; } = null!;

        
        public ICollection<ProductVariantPropertyValue> Properties { get; set; } = new List<ProductVariantPropertyValue>();
    }

    

    public class ProductPropertyValue
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid CategoryPropertyId { get; set; }

        [Required, StringLength(500)]
        public string Value { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(CategoryPropertyId))]
        public CategoryProperty CategoryProperty { get; set; } = null!;
    }

    public class ProductVariantPropertyValue
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductVariantId { get; set; }

        [Required]
        public Guid CategoryPropertyId { get; set; }

        [Required, StringLength(500)]
        public string Value { get; set; } = null!;

        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant Variant { get; set; } = null!;

        [ForeignKey(nameof(CategoryPropertyId))]
        public CategoryProperty CategoryProperty { get; set; } = null!;
    }

    // ----------------------------
    // Customer
    // ----------------------------
    public class Customer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicantUser User { get; set; } = null!;

        [StringLength(200)]
        public string? FullName { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

    // ----------------------------
    // Cart Item
    // ----------------------------
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid ProductVariantId { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant ProductVariant { get; set; } = null!;

        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;
    }

    // ----------------------------
    // Orders
    // ----------------------------
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public Guid? DeliveryId { get; set; }

        [ForeignKey(nameof(DeliveryId))]
       
        public Delivery? Delivery { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid ProductVariantId { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant ProductVariant { get; set; } = null!;

        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;
    }

    // ----------------------------
    // Delivery
    // ----------------------------
    public class Delivery
    {
        [Key]
        public Guid DeliveryId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderId { get; set; }

        [Required, StringLength(50)]
        public string DeliveryStatus { get; set; } = DeliveryStatusEnum.Pending.ToString();

        [Column(TypeName = "datetime2")]
        public DateTime? DeliveryDate { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
    }

    
}
