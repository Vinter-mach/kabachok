using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Cs_backend.Repositories;

public class StudentRepository(ApplicationContext applicationContext)
    : BaseRepository<Student>(applicationContext, applicationContext.Students)
{
}