using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;
using ICSProject.Common.Tests;
using ICSProject.Common.Tests.Seeds;
using ICSProject.DAL.Entities;
using ICSProject.DAL.Mappers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;

namespace ICSProject.BL.Tests;
public sealed class UserFacadeTest : FacadeTestBase
{
    private readonly IUserFacade _userFacadeSUT;

    public UserFacadeTest(ITestOutputHelper output) : base(output)
    {
        _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserEntityMapper, UserModelMapper);
    }

    [Fact]
    public async Task GetFromAllSeeded()
    {
        var Users = await _userFacadeSUT.GetAsync();
        var User = Users.Single(i => i.Id == UserSeeds.UserEntity.Id);
        DeepAssert.Equal(UserModelMapper.MapToListModel(UserSeeds.UserEntity), User);
    }

    [Fact]
    public async Task GetByIdNonExistent()
    {
        var User = await _userFacadeSUT.GetAsync(UserSeeds.EmptyUserEntity.Id);
        Assert.Null(User);
    }

    [Fact]
    public async Task SeedUserDeleteDeleted()
    {
        await _userFacadeSUT.DeleteAsync(UserSeeds.UserEntityWithNoActivities.Id);
        await using var dbAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbAssert.Users.AnyAsync(i => i.Id == UserSeeds.UserEntityWithNoActivities.Id));
    }
}

