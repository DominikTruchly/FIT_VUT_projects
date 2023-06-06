using ICSProject.BL.Facades;
using ICSProject.BL.Models;
using ICSProject.Common.Tests;
using ICSProject.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ICSProject.DAL.Mappers;
using Xunit;
using Xunit.Abstractions;

namespace ICSProject.BL.Tests;

public sealed class ActivityFacadeTest : FacadeTestBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    public ActivityFacadeTest(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper, ActivityEntityMapper);
    }

    [Fact]
    public async Task GetFromAllSeeded()
    {
        var Activities = await _activityFacadeSUT.GetAsync();
        var Activity = Activities.Single(i => i.Id == ActivitySeeds.Activity.Id);
        DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Activity), Activity);
    }

    [Fact]
    public async Task GetByIdActivity()
    {
        var Activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.Activity.Id);

        DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivitySeeds.Activity), Activity);
    }

    [Fact]
    public async Task GetByIdNonExistent()
    {
        var Activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.EmptyActivity.Id);
        Assert.Null(Activity);
    }

    [Fact]
    public async Task SeedWorkDeleteDeleted()
    {
        await _activityFacadeSUT.DeleteAsync(ActivitySeeds.Activity.Id);

        await using var dbAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbAssert.Activities.AnyAsync(i => i.Id == ActivitySeeds.Activity.Id));
    }
}
