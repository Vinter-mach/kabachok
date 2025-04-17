using System.Linq.Expressions;
using Cs_backend.Interfaces;
using Cs_backend.Repositories;

namespace Cs_backend.Services.BaseServices;

public static class GetterByFunc
{
    public static async Task<List<TOut>> GetByFunc<TIn, TOut>(IRepository<TIn> repository, Expression<Func<TIn, bool>> func)
        where TIn : class, ICastable<TOut>
    {
        var entities = await repository.GetByFuncAsync(func);
        return entities.Select(x => x.Cast()).ToList();
    }
}