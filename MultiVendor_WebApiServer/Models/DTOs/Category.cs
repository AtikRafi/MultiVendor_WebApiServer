namespace MultiVendor_WebApiServer.Models.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; } = null!;
        public List<CategoryPropertyDto> Properties { get; set; } = new();
    }
    public class CategoryPropertyDto
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public PropertyDataType Datatype { get; set; }
        public bool IsVariant { get; set; }

    }
}
