using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddOrUpdateAsync(T entity);
    void DeleteAsync(T entity);
}