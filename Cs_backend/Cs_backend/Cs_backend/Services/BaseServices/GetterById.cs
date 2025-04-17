using Cs_backend.Interfaces;
using Cs_backend.Repositories;

namespace Cs_backend.Services.BaseServices;

public static class GetterById
{
    public static async Task<List<TOut>> GetByIdAsync<TIn, TOut>(IRepository<TIn> repository, int id)
        where TIn : class, ICastable<TOut>
    {
        var courses = await repository.GetAllAsync();
        return courses.Select(course => course.Cast()).ToList();
    }
}