using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Cs_backend.Repositories;

public class AuthRepository(ApplicationContext applicationContext)
    : BaseRepository<SubmittedTask>(applicationContext, applicationContext.SubmittedTasks)
{
    public async Task<Teacher?> GetByLoginAsync(string login)
    {
        return await applicationContext.Teachers.FirstOrDefaultAsync(t => t.Login == login);
    }
}
