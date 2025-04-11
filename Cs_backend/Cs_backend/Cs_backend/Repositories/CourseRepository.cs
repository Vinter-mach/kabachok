using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Repositories;

public class CourseRepository(ApplicationContext applicationContext) : IRepository<Course>
{
    public async Task<Course?> GetByIdAsync(int id)
    {
        return await applicationContext.Courses.FindAsync(id);
    }

    public async Task<List<Course?>> GetAllAsync()
    {
        return await applicationContext.Courses.AsNoTracking().ToListAsync();
    }

    public async Task AddOrUpdateAsync(Course entity)
    {
        var existing = await applicationContext.Courses.FindAsync(entity.Id);
        if (existing == null)
            applicationContext.Courses.Add(entity);
        else
            applicationContext.Entry(existing).CurrentValues.SetValues(entity);

        await applicationContext.SaveChangesAsync();
    }

    public async void DeleteAsync(Course entity)
    {
        var existing = await applicationContext.Courses.FindAsync(entity.Id);
        if (existing == null)
            return;
        applicationContext.Courses.Remove(existing);
        await applicationContext.SaveChangesAsync();
    }
}
