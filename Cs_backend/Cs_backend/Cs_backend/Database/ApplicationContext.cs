using Microsoft.EntityFrameworkCore;


namespace Cs_backend.Database;

public class ApplicationContext : DbContext
{
    //TODO 
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    {
        throw new NotImplementedException();
    }
}