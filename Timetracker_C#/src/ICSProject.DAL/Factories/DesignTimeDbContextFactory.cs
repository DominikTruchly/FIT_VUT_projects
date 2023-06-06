namespace ICSProject.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Identity.Client.Extensions.Msal;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ICSProjectDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory;
    private const string ConnectionString = $"Data Source=ICSProject.db;Cache=Shared";

    public DesignTimeDbContextFactory()
    {
        this._dbContextSqLiteFactory = new DbContextSqLiteFactory(ConnectionString);
    }

    public ICSProjectDbContext CreateDbContext(string[] args) => this._dbContextSqLiteFactory.CreateDbContext();

}
