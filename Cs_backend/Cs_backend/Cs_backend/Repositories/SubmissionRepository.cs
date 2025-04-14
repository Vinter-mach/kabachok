using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;
using Task = Cs_backend.Models.Task;

namespace Cs_backend.Repositories;

public class SubmissionRepository(ApplicationContext applicationContext)
    : BaseRepository<SubmittedTask>(applicationContext, applicationContext.SubmittedTasks)
{
}