using Cs_backend.DTO;
using Cs_backend.Interfaces;
using Cs_backend.Repositories;

namespace Cs_backend.Services.BaseServices;

public static class AllGetter
{
    public static async Task<List<TOut>> GetAllAsync<TIn, TOut>(IRepository<TIn> repository) where TIn : class, ICastable<TOut>
    {
        var entities = await repository.GetAllAsync();
        return entities.Select(course => course.Cast()).ToList();
    }
}