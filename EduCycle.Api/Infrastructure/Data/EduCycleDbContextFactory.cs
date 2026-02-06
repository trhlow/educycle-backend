using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EduCycle.Api.Infrastructure.Data;

public class EduCycleDbContextFactory
    : IDesignTimeDbContextFactory<EduCycleDbContext>
{
    public EduCycleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EduCycleDbContext>();

        // HARD-CODE connection string (DESIGN-TIME ONLY)
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=EduCycleDb;Trusted_Connection=True;TrustServerCertificate=True"
        );

        return new EduCycleDbContext(optionsBuilder.Options);
    }
}
