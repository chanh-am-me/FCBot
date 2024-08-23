using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistents;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
