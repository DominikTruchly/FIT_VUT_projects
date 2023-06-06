using ICSProject.App.Options;
using ICSProject.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; 

namespace ICSProject.App;

interface IDbMigrator
{
    public void Migrate();
    public Task MigrateAsync(CancellationToken cancellationToken);
}

public class NoneDbMigrator : IDbMigrator
{
    public void Migrate() { }
    public Task MigrateAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public class SqliteDbMigrator : IDbMigrator
{
    private readonly IDbContextFactory<ICSProjectDbContext> _dbContextFactory;
    private readonly SqliteOptions _sqliteOptions;

    public SqliteDbMigrator(IDbContextFactory<ICSProjectDbContext> dbContextFactory, DALOptions dalOptions)
    {
        _dbContextFactory = dbContextFactory;
        _sqliteOptions = dalOptions.Sqlite ??
                         throw new ArgumentNullException(nameof(dalOptions),
                             $@"{nameof(DALOptions.Sqlite)} are not set.");
    }
    public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using ICSProjectDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        if (_sqliteOptions.RecreateDatabaseEachTime)
        {
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

    }
}


