using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Cs_backend.Repositories;

public class StudentRepository(ApplicationContext applicationContext)
    : BaseRepository<Student>(applicationContext, applicationContext.Students)
{
    public async Task<List<Student>> GetStudentsByGroupId(int groupId)
    {
        return await applicationContext.Students.AsNoTracking().Where(t => t.GroupId == groupId).ToListAsync();
    }
}