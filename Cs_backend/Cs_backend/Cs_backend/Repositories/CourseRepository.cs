using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Cs_backend.Repositories;

public class CourseRepository(ApplicationContext applicationContext)
    : BaseRepository<Course>(applicationContext, applicationContext.Courses)
{
}