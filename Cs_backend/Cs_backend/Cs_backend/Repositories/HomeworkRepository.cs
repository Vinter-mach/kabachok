using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Repositories;

public class HomeworkRepository(ApplicationContext applicationContext)
    : BaseRepository<SubmittedTask>(applicationContext, applicationContext.SubmittedTasks)
{
    public async Task<SubmittedTask?> GetHomeworkById(int submissionId)
    {
        return await applicationContext.SubmittedTasks.AsNoTracking().Where(t => t.Id == submissionId).FirstOrDefaultAsync();
    }
    
    public async Task UpdateAsync(SubmittedTask entity)
    {
        var existing = await applicationContext.SubmittedTasks.FindAsync(entity.Id);
        if (existing == null)
            return;

        existing.Comment = entity.Comment;
        existing.Grade = entity.Grade;
        entity.StatusId = entity.StatusId;

        await applicationContext.SaveChangesAsync();
    }
}