using Cs_backend.Database;
using Cs_backend.Models;

public class BaseRepository<TDbModel>(ApplicationContext context)
    where TDbModel : class
{
    private ApplicationContext Context { get; set; } = context;
}