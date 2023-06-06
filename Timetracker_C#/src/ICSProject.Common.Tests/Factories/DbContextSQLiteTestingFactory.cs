using ICSProject.DAL;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.Common.Tests.Factories;
public class DbContextSQLiteTestingFactory : IDbContextFactory<ICSProjectDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;
    public DbContextSQLiteTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public ICSProjectDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<ICSProjectDbContext> builder = new();
        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

        return new ICSProjectTestingDbContext(builder.Options, _seedTestingData);
    }
}
