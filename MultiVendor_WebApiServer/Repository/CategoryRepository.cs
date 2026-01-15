using Microsoft.EntityFrameworkCore;
using MultiVendor_WebApiServer.Models;
using MultiVendor_WebApiServer.Models.DTOs;

namespace MultiVendor_WebApiServer.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetWithProperties(Guid id);
        Task Create(CategoryDto dto);
        Task Update(Guid id, CategoryDto dto);
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetWithProperties(Guid id)
        {
            return await Context.Categories
                                .Include(c => c.Properties)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Create(CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
            };

            foreach (var prop in dto.Properties)
            {
                category.Properties.Add(new CategoryProperty
                {
                    Name = prop.Name,
                    DataType = prop.Datatype,
                    IsVariant = prop.IsVariant
                });
            }

            await AddAsync(category);
           
        }


        public async Task Update(Guid id, CategoryDto dto)
        {
            
            var category = await Context.Categories
                .Include(c => c.Properties)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new Exception("Category not found");

            
            category.Name = dto.Name;

           
            var incomingIds = dto.Properties
                .Where(p => p.Id.HasValue)
                .Select(p => p.Id!.Value)
                .ToList(); 

            
            var toRemove = category.Properties
                .Where(p => !incomingIds.Contains(p.Id))
                .ToList();

            foreach (var prop in toRemove)
            {
                category.Properties.Remove(prop);
               
            }

           
            foreach (var pDto in dto.Properties)
            {
                if (pDto.Id.HasValue)
                {
                    
                    var existingProp = category.Properties
                        .SingleOrDefault(p => p.Id == pDto.Id.Value);

                    if (existingProp != null)
                    {
                        existingProp.Name = pDto.Name;
                        existingProp.DataType = pDto.Datatype;
                        existingProp.IsVariant = pDto.IsVariant;
                    }
                    
                }
                else
                {
                    
                    category.Properties.Add(new CategoryProperty
                    {
                        CategoryId = category.Id, 
                        Name = pDto.Name,
                        DataType = pDto.Datatype,
                        IsVariant = pDto.IsVariant
                    });
                }
            }
            //UpdateAsync(category);
            // SaveChanges will now:
            // - UPDATE Category
            // - DELETE removed CategoryProperties (detected automatically)
            // - INSERT new ones
            // - UPDATE modified ones
            //await Context.SaveChangesAsync();
        }

    }
}
