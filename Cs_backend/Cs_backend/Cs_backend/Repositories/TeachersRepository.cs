using Cs_backend.Database;
using Cs_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Cs_backend.Repositories;

public class TeachersRepository(ApplicationContext applicationContext)
    : BaseRepository<Teacher>(applicationContext, applicationContext.Teachers)
{
}