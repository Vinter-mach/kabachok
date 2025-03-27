using Microsoft.EntityFrameworkCore;


namespace Cs_backend.Database;

public class ApplicationContext : DbContext
{
    //TODO - Requires database description
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    {
        throw new NotImplementedException();
    }
}