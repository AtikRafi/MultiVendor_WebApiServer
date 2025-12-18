namespace MultiVendor_WebApiServer.Models.DTOs
{
    public class ProductCreateRequest
    {
        public String Name { get; set; } =null!;
        public String? BrandName { get; set; }
        public string Description { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool HasVariants { get; set; }
        public string ProductPropertiesJson { get; set; } = null!;
        public string? VariantsJson { get; set; }
        public List<IFormFile>? ProductImages { get; set; }
        public Dictionary<int, List<IFormFile>> VariantImages { get; set; } = new();
    }
    public class ProductCreateDto
    {
     public String Name { get; set; } =null!;
        public String? BrandName { get; set; }
        public string Description { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool HasVariants { get; set; }
        public List<ProductPropertyDto> ProductProperties { get; set; } = [];
        public List<ProductVariantDto>? Variants { get; set; }
        public List<ProductImageDto> ProductImageUrls { get; set; } = [];
        
    }

    public class ProductImageDto
    {
        public string FilePath { get; set; }=null!;
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }

    public class ProductVariantDto
    {
        public int SerialNumber { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public Decimal? CompareAtPrice { get; set; }
        public int Stock { get; set; }
        public bool IsDefault { get; set; }
        
        public List<ProductVariantPropertyDto> Properties { get; set; } = [];
        public List<ProductVariantImageDto> Images { get; set; } = [];

    }

    public class ProductVariantImageDto
    {
        public string FilePath { get; set; }=null!;
        public int DisplayOrder { get; set; }
        
    }

    public class ProductVariantPropertyDto
    {
        public  Guid CategoryPropertyId { get; set; }
        public string Value { get; set; } = null!;
    }

    public class ProductPropertyDto
    {
        public Guid CategoryPropertyId { get; set; }
        public string Value { get; set; } = null!;
    }

}
