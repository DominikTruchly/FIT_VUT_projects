using ICSProject.BL.Facades;
using ICSProject.BL.Models;
using ICSProject.Common.Tests;
using ICSProject.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ICSProject.BL.Tests;
public sealed class ProjectFacadeTest : FacadeTestBase
{
    private readonly IProjectFacade _projectFacadeSUT;

    public ProjectFacadeTest(ITestOutputHelper output) : base(output)
    {
        _projectFacadeSUT = new ProjectFacade(UnitOfWorkFactory, ProjectModelMapper, ActivityFacade, UserFacade, ProjectEntityMapper, UserModelMapper);
    }

    [Fact]
    public async Task GetFromAllSeeded()
    {
        var Projects = await _projectFacadeSUT.GetAsync();
        var Project = Projects.Single(i => i.Id == ProjectSeeds.ProjectEntity.Id);
        DeepAssert.Equal(ProjectModelMapper.MapToListModel(ProjectSeeds.ProjectEntity), Project);
    }

    [Fact]
    public async Task GetByIdProject()
    {
        var Project = await _projectFacadeSUT.GetAsync(ProjectSeeds.ProjectEntity.Id);

        DeepAssert.Equal(ProjectModelMapper.MapToDetailModel(ProjectSeeds.ProjectEntity), Project);
    }

    [Fact]
    public async Task GetByIdNonExistent()
    {
        var Project = await _projectFacadeSUT.GetAsync(ProjectSeeds.EmptyProjectEntity.Id);
        Assert.Null(Project);
    }

    [Fact]
    public async Task SeedProjDeleteDeleted()
    {
        await _projectFacadeSUT.DeleteAsync(ProjectSeeds.ProjectEntity.Id);

        await using var dbAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbAssert.Activities.AnyAsync(i => i.Id == ProjectSeeds.ProjectEntity.Id));
    }
}
