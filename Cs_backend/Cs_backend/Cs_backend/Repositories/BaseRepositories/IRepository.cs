using System.Linq.Expressions;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetByFuncAsync(Expression<Func<T, bool>> predicate); //Expression нужен для того, чтобы все было норм в EF

    Task<List<T>> GetAllAsync();
    Task AddOrUpdateAsync(T entity);
    void DeleteAsync(T entity);
}