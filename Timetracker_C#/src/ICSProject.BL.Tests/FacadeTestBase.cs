using ICSProject.BL.Mappers;
using ICSProject.Common.Tests;
using ICSProject.Common.Tests.Factories;
using ICSProject.DAL;
using ICSProject.DAL.Mappers;
using ICSProject.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ICSProject.BL.Facades;
using Xunit;
using Xunit.Abstractions;

namespace ICSProject.BL.Tests;
public class FacadeTestBase : IAsyncLifetime
{
    protected FacadeTestBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        ActivityModelMapper = new ActivityModelMapper();
        UserModelMapper = new UserModelMapper(ActivityModelMapper, ProjectModelMapper);
        ProjectModelMapper = new ProjectModelMapper(ActivityModelMapper);

        UserEntityMapper = new UserEntityMapper();
        ProjectEntityMapper = new ProjectEntityMapper();
        ActivityEntityMapper = new ActivityEntityMapper();

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);

        UserFacade = new UserFacade(UnitOfWorkFactory, UserEntityMapper, UserModelMapper);
        ActivityFacade = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper, ActivityEntityMapper);
    }
    protected IDbContextFactory<ICSProjectDbContext> DbContextFactory { get;}

    protected IActivityModelMapper ActivityModelMapper { get;}
    protected IProjectModelMapper ProjectModelMapper { get;}
    protected IUserModelMapper UserModelMapper { get;}

    protected UserEntityMapper UserEntityMapper { get;}
    protected ActivityEntityMapper ActivityEntityMapper { get;}
    protected ProjectEntityMapper ProjectEntityMapper { get;}

    protected UserFacade UserFacade { get;}
    protected ActivityFacade ActivityFacade { get;}

    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}
