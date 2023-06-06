namespace ICSProject.DAL.Factories;
using System;
using Microsoft.EntityFrameworkCore;


public class DbContextSqLiteFactory : IDbContextFactory<ICSProjectDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<ICSProjectDbContext> _contextOptionsBuilder = new();
     

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;

        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }
    
    public ICSProjectDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedTestingData);
}
