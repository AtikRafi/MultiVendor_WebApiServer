using Microsoft.EntityFrameworkCore;
using MultiVendor_WebApiServer.Models;
using System.Threading.Tasks;

namespace MultiVendor_WebApiServer.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        void UpdateAsync(T entity);
        void RemoveAsync(T entity);  
    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<T> dbset;
        public GenericRepository(AppDbContext context)
        {
            this.Context = context;
            dbset = context.Set<T>();
        }
        public async Task AddAsync(T entity) => await dbset.AddAsync(entity);

        public async Task<IEnumerable<T>> GetAllAsync() => await dbset.ToListAsync();
        
        public async Task<T?> GetByIdAsync(Guid id) => await dbset.FindAsync(id);

        public void RemoveAsync(T entity) => dbset.Remove(entity);

        public void UpdateAsync(T entity) => dbset.Update(entity);
    }
}
