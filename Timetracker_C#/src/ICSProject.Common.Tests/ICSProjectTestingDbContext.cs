using ICSProject.Common.Tests.Seeds;
using ICSProject.DAL;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.Common.Tests;
public class ICSProjectTestingDbContext : ICSProjectDbContext
{
    private readonly bool _seedTestingData;

    public ICSProjectTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, seedDemoData: false)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedTestingData)
        {
            UserSeeds.Seed(modelBuilder);
            ProjectSeeds.Seed(modelBuilder);
            ActivitySeeds.Seed(modelBuilder);
        }
    }
}
