//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using MultiVendor_WebApiServer.Models.DTOs;
//using MultiVendor_WebApiServer.Services;
//using System.ComponentModel.DataAnnotations;
//using System.Text.Json;

//namespace MultiVendor_WebApiServer.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductController : ControllerBase
//    {
//        private readonly IUnitOfWork uow;
//        private readonly IWebHostEnvironment env;

//        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
//        {
//            this.uow = unitOfWork;
//            this.env = env;
//        }

//        [HttpPost]
//        [Consumes("multipart/form-data")]
//        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
//        {
//            try
//            {
//                // 1️⃣ Deserialize product properties
//                var productProps = JsonSerializer.Deserialize<List<ProductPropertyDto>>(request.ProductPropertiesJson)
//                                   ?? new List<ProductPropertyDto>();

//                // 2️⃣ Deserialize variants
//                var variants = string.IsNullOrWhiteSpace(request.VariantsJson) ? null :
//                               JsonSerializer.Deserialize<List<ProductVariantDto>>(request.VariantsJson);

//                // 3️⃣ Save product images
//                var productImages = new List<ProductImageDto>();
//                if (request.ProductImages != null)
//                {
//                    for (int i = 0; i < request.ProductImages.Count; i++)
//                    {
//                        var file = request.ProductImages[i];
//                        var path = await SaveFileAsync(file);
//                        productImages.Add(new ProductImageDto
//                        {
//                            FilePath = path,
//                            DisplayOrder = i,
//                            IsMain = i == 0
//                        });
//                    }
//                }

//                // 4️⃣ Assign variant images
//                if (variants != null && request.VariantImages != null)
//                {
//                    foreach (var kv in request.VariantImages)
//                    {
//                        var idx = kv.Key;
//                        var files = kv.Value ?? new List<IFormFile>();

//                        if (idx >= variants.Count)
//                            return BadRequest($"Variant index {idx} is out of range.");

//                        variants[idx].Images = new List<ProductVariantImageDto>();
//                        for (int i = 0; i < files.Count; i++)
//                        {
//                            variants[idx].Images.Add(new ProductVariantImageDto
//                            {
//                                FilePath = await SaveFileAsync(files[i]),
//                                DisplayOrder = i
//                            });
//                        }
//                    }
//                }

//                // 5️⃣ Create DTO
//                var dto = new ProductCreateDto
//                {
//                    Name = request.Name,
//                    BrandName = request.BrandName,
//                    Description = request.Description,
//                    CategoryId = request.CategoryId,
//                    HasVariants = request.HasVariants,
//                    ProductProperties = productProps,
//                    Variants = variants,
//                    ProductImageUrls = productImages
//                };

//                //var vendorId = User.GetVendorId();

//                // 6️⃣ Create product (repository handles transaction)
//                //var productId = await uow.Products.CreateAsync(dto, vendorId);
//                //var productId = await uow.Products.CreateAsync(dto);

//                return Ok(new { ProductId = productId });
//            }
//            catch (ValidationException vex)
//            {
//                return BadRequest(new { Error = vex.Message });
//            }
//            catch (Exception ex)
//            {
//                // Log exception (optional)
//                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
//            }
//        }

//        // Async file save
//        private async Task<string> SaveFileAsync(IFormFile file)
//        {
//            var uploadsFolder = Path.Combine(env.WebRootPath ?? "wwwroot", "uploads", "products");
//            if (!Directory.Exists(uploadsFolder))
//                Directory.CreateDirectory(uploadsFolder);

//            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
//            var filePath = Path.Combine(uploadsFolder, fileName);

//            await using var stream = new FileStream(filePath, FileMode.Create);
//            await file.CopyToAsync(stream);

//            // Return relative path for DB storage
//            return Path.Combine("uploads", "products", fileName).Replace("\\", "/");
//        }
//    }
//}
