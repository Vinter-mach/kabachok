using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Repositories;

public abstract class BaseRepository<T>(ApplicationContext applicationContext, DbSet<T> dbSet)
    : IRepository<T> where T : BaseModel
{
    public async Task<T?> GetByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await dbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddOrUpdateAsync(T entity)
    {
        var existing = await dbSet.FindAsync(entity.Id);
        if (existing == null)
            dbSet.Add(entity);
        else
            applicationContext.Entry(existing).CurrentValues.SetValues(entity);

        await applicationContext.SaveChangesAsync();
    }

    public async void DeleteAsync(T entity)
    {
        var existing = await dbSet.FindAsync(entity.Id);
        if (existing == null)
            return;
        dbSet.Remove(existing);
        await applicationContext.SaveChangesAsync();
    }
}