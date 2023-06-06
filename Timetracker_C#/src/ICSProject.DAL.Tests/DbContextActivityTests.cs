using ICSProject.DAL;
using ICSProject.DAL.Entities;
using ICSProject.DAL.Factories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace ICSProject.DAL.Tests;

public class DbContextActivityTests
{
    private readonly DbContextOptions<ICSProjectDbContext> _dbContextOptions;
    public DbContextActivityTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        _dbContextOptions = new DbContextOptionsBuilder<ICSProjectDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new ICSProjectDbContext(_dbContextOptions);
        context.Database.EnsureCreated();
    }

    [Fact]
    public void AddActivityToUser_ActivityAddedToUser()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            ImageUrl = null
        };
        context.Users.Add(user);
        context.SaveChanges();
        user = context.Users.Single(u => u.Id == user.Id);

        var activity = new ActivityEntity()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Odcitavanie vodomerov",
            Type = "Work",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "Do some work"
        };

        context.Activities.Add(activity);
        user.UsrActivities.Add(activity);
        context.SaveChanges();

        var userWithActivity = context.Users.Include(u => u.UsrActivities).FirstOrDefault(u => u.Id == user.Id);
        Assert.NotNull(userWithActivity?.UsrActivities);
        Assert.Contains(activity, userWithActivity.UsrActivities);
    }

    [Fact]
    public void UpdateActivityOfUser_ActivityUpdated()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            ImageUrl = null
        };
        context.Users.Add(user);
        context.SaveChanges();
        user = context.Users.Single(u => u.Id == user.Id);

        var activity = new ActivityEntity()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Odcitavanie vodomerov",
            Type = "Work",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "Do some work"
        };
        context.Activities.Add(activity);
        user.UsrActivities.Add(activity);
        context.SaveChanges();

        var updatedActivity = new ActivityEntity()
        {
            Id = activity.Id,
            UserId = user.Id,
            Name = "Leisure",
            Type = "Leisure",
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now.AddDays(3),
            Description = "Relax and have fun"
        };
        context.Entry(activity).CurrentValues.SetValues(updatedActivity);
        context.SaveChanges();

        var userWithActivity = context.Users.Include(u => u.UsrActivities).FirstOrDefault(u => u.Id == user.Id);
        Assert.NotNull(userWithActivity?.UsrActivities);
        var activityInDb = userWithActivity.UsrActivities.FirstOrDefault(a => a.Id == activity.Id);
        Assert.NotNull(activityInDb);
        Assert.Equal(updatedActivity.Type, activityInDb.Type);
        Assert.Equal(updatedActivity.StartDate, activityInDb.StartDate);
        Assert.Equal(updatedActivity.EndDate, activityInDb.EndDate);
        Assert.Equal(updatedActivity.Description, activityInDb.Description);
    }

    [Fact]
    public void GetActivitiesOfUser_ActivitiesRetrieved()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            ImageUrl = null
        };
        context.Users.Add(user);
        context.SaveChanges();
        user = context.Users.Single(u => u.Id == user.Id);

        var activity1 = new ActivityEntity()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Work",
            Type = "Work",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "Do some work"
        };
        context.Activities.Add(activity1);
        user.UsrActivities.Add(activity1);

        var activity2 = new ActivityEntity()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Leisure",
            Type = "Leisure",
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(2),
            Description = "Relax and have fun"
        };
        context.Activities.Add(activity2);
        user.UsrActivities.Add(activity2);

        context.SaveChanges();

        var users = context.Users.Where(a => a.Id == user.Id).ToList();

        Assert.NotNull(users);
        Assert.Contains(activity1, users[0].UsrActivities);
        Assert.Contains(activity2, users[0].UsrActivities);
    }

    [Fact]
    public void DeleteActivity_ActivityDeletedFromDatabase()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            ImageUrl = null
        };
        context.Users.Add(user);
        context.SaveChanges();
        user = context.Users.Single(u => u.Id == user.Id);

        var activity = new ActivityEntity()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Work",
            Type = "Work",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "Do some work"
        };

        context.Activities.Add(activity);
        user.UsrActivities.Add(activity);
        context.SaveChanges();

        context.Activities.Remove(activity);
        user.UsrActivities.Remove(activity);
        context.SaveChanges();

        var userWithActivity = context.Users.Include(u => u.UsrActivities).FirstOrDefault(u => u.Id == user.Id);
        Assert.NotNull(userWithActivity!.UsrActivities);
        Assert.DoesNotContain(activity, userWithActivity.UsrActivities);
        Assert.Null(context.Activities.Find(activity.Id));
    }

    [Fact]
    public void CreateActivityForProject_ActivityCreatedForProject()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe"
        };

        var activity = new ActivityEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Activity",
            Type = "Test Activity",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "This is a test activity"
        };
        user.UsrActivities.Add(activity);

        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Project",
            Description = "This is a test project"
        };
        project.ProjActivities.Add(activity);


        context.Users.Add(user);
        context.Projects.Add(project);
        context.Activities.Add(activity);
        context.SaveChanges();

        var users = context.Users.FirstOrDefault(a => a.Id == user.Id);
        var projects = context.Projects.FirstOrDefault(a => a.Id == project.Id);
        Assert.NotNull(users);
        Assert.NotNull(projects);
        Assert.Equal(activity.Id, users.UsrActivities.ToList()[0].Id);
        Assert.Equal(activity.Id, project.ProjActivities.ToList()[0].Id);
    }

    [Fact]
    public void UpdateActivityForProject_ActivityUpdatedForProject()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe"
        };
        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Project",
            Description = "This is a test project"
        };
        var activity = new ActivityEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Activity",
            Type = "Test Activity",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "This is a test activity"
        };
        context.Users.Add(user);
        context.Projects.Add(project);
        context.Activities.Add(activity);
        context.SaveChanges();

        var updatedActivity = context.Activities.FirstOrDefault(a => a.Id == activity.Id);
        updatedActivity!.Type = "Updated Test Activity";
        updatedActivity.Description = "This is an updated test activity";
        context.SaveChanges();

        var activityFromDb = context.Activities.FirstOrDefault(a => a.Id == activity.Id);
        Assert.NotNull(activityFromDb);
        Assert.Equal(updatedActivity.Type, activityFromDb.Type);
        Assert.Equal(updatedActivity.Description, activityFromDb.Description);
    }

    [Fact]
    public void DeleteActivity_ActivityDeleted()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe"
        };

        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Project",
            Description = "This is a test project"
        };

        var activity = new ActivityEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Activity",
            Type = "Test Activity",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "This is a test activity"
        };

        context.Users.Add(user);
        context.Projects.Add(project);
        context.Activities.Add(activity);
        context.SaveChanges();

        context.Activities.Remove(activity);
        context.SaveChanges();

        var deletedActivity = context.Activities.FirstOrDefault(a => a.Id == activity.Id);
        Assert.Null(deletedActivity);
    }

    [Fact]
    public void GetActivitiesFromProject_AllActivitiesReturned()
    {
        using var context = new ICSProjectDbContext(_dbContextOptions);

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe"
        };

        var activity1 = new ActivityEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Activity 1",
            Type = "Test Activity 1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            Description = "This is a test activity 1"
        };
        var activity2 = new ActivityEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Activity 2",
            Type = "Test Activity 2",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            Description = "This is a test activity 2"
        };

        user.UsrActivities.Add(activity1);
        user.UsrActivities.Add(activity2);

        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Project",
            Description = "This is a test project"
        };
        project.ProjActivities.Add(activity1);
        project.ProjActivities.Add(activity2);

        context.Users.Add(user);
        context.Projects.Add(project);
        context.Activities.AddRange(activity1, activity2);
        context.SaveChanges();

        var projects = context.Projects.Where(a => a.Id == project.Id).ToList();

        Assert.NotNull(projects);
        Assert.Equal(projects[0].ProjActivities.ToList()[0].Id, activity1.Id);
        Assert.Equal(projects[0].ProjActivities.ToList()[1].Id, activity2.Id);

    }

}
