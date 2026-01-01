//using MultiVendor_WebApiServer.Models;
//using MultiVendor_WebApiServer.Models.DTOs;
//using MultiVendor_WebApiServer.Services;

//namespace MultiVendor_WebApiServer.Repository
//{
//    public interface IProductRepository : IGenericRepository<Product>
//    {
//        //Task<Guid> CreateAsync(ProductCreateDto productDto, Guid VendorId);
//        Task<Guid> CreateAsync(ProductCreateDto productDto);
//    }
//    public class ProductRepository : GenericRepository<Product>, IProductRepository
//    {
//        public ProductRepository(AppDbContext context) : base(context)
//        {
//        }

//        public async Task<Guid> CreateAsync(ProductCreateDto productDto)
//        {
//            var product = new Product
//            {
//                Name = productDto.Name,
//                BrandName = productDto.BrandName,
//                Description = productDto.Description,
//                //fix CategoryId and VendorId later
//                CategoryId = Guid.NewGuid(),
//                VendorId = Guid.NewGuid(),
//                IsPublished = false,
//                CreatedAt = DateTime.UtcNow,
//            };
//            //images
//            foreach (var i in productDto.ProductImageUrls)
//            {
//                product.Images.Add(new ProductImage
//                {
//                    PicturePath = i.FilePath,
//                    DisplayOrder = i.DisplayOrder,
//                    IsMain = i.IsMain
//                });
//            }
//            //non-variant properties
//            foreach (var p in productDto.ProductProperties)
//            {
//                var catProp = await Context.CategoryProperties.FindAsync(p.CategoryPropertyId) ?? throw new Exception("Invalid Property");
               
//                if (catProp.IsVariant)
//                    throw new Exception($"{catProp.Name} must be Variant Level");

              
//                PropertyValueValidator.ValidatePropertyValue(catProp, p.Value);
                
//                product.PropertyValues.Add(new ProductPropertyValue
//                {
//                    CategoryPropertyId = p.CategoryPropertyId,
//                    Value = p.Value
//                });
//            }
//            //variant properties
//            if (productDto.HasVariants && productDto.Variants != null)
//            {
//                foreach(var v in productDto.Variants.OrderBy(x=>x.SerialNumber))
//                {
//                    var variant = new ProductVariant
//                    {
//                        Sku = v.SKU,
//                        Price = v.Price,
//                        CompareAtPrice = v.CompareAtPrice,
//                        Stock = v.Stock,
//                        IsDefault = v.IsDefault
//                    };
//                    //variant Images
//                    foreach (var img in v.Images)
//                    {
//                        variant.Images.Add(new ProductVariantImage
//                        {
//                            PicturePath = img.FilePath,
//                            DisplayOrder = img.DisplayOrder
//                        });
//                    }
//                    //variant Properties
//                    foreach(var vp in v.Properties)
//                    {
//                        var catProp = await Context.CategoryProperties.FindAsync(vp.CategoryPropertyId) ?? throw new Exception("Invalid Property");

//                        if (!catProp.IsVariant)
//                        {
//                            throw new Exception($"{catProp.Name} must be product-level");
//                        }
//                        PropertyValueValidator.ValidatePropertyValue(catProp, vp.Value);

//                        variant.Properties.Add(new ProductVariantPropertyValue
//                        {
//                            CategoryPropertyId = vp.CategoryPropertyId,
//                            Value = vp.Value
//                        });

//                    }
//                    product.Variants.Add(variant);

//                }
//            }
//            await AddAsync(product);

//            return product.Id;
//        }

//    }
//}
