using MultiVendor_WebApiServer.Models;
using MultiVendor_WebApiServer.Repository;

namespace MultiVendor_WebApiServer.Services
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        //IProductRepository Products { get; }
        Task<int> SaveAsAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public ICategoryRepository Categories { get; }
        //public IProductRepository Products { get; } 

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            Categories = new CategoryRepository(context);
            //Products = new ProductRepository(context);
        }

        public async Task<int> SaveAsAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
