namespace ICSProject.DAL.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class SqlServerDbContextFactory : IDbContextFactory<ICSProjectDbContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<ICSProjectDbContext> _contextOptionsBuilder = new();

    public SqlServerDbContextFactory(string connectionString, bool seedDemoData = false)
    {
        _seedDemoData = seedDemoData;

        _contextOptionsBuilder.UseSqlServer(connectionString);
    }

    public ICSProjectDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
}
