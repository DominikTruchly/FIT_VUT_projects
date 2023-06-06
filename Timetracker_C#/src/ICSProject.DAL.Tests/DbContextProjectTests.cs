using ICSProject.DAL;
using ICSProject.DAL.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.DAL.Tests;

public class DbContextProjectTests
{
    private readonly DbContextOptions<ICSProjectDbContext> DbContextOptions;
    public DbContextProjectTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        DbContextOptions = new DbContextOptionsBuilder<ICSProjectDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new ICSProjectDbContext(DbContextOptions);
        context.Database.EnsureCreated();
    }

    [Fact]
    public void AddProjectToUser_ProjectAddedToUser()
    {
        using var context = new ICSProjectDbContext(DbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "James",
            Surname = "Adams",
            ImageUrl = null
        };

        context.Users.Add(user);
        context.SaveChanges();

        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Test Project",
            Description = "This is a test project"
        };

        context.Projects.Add(project);
        user.UsrProjects.Add(project);
        context.SaveChanges();

        var addedProject = context.Projects.FirstOrDefault(p => p.Id == project.Id);
        Assert.NotNull(addedProject);
        Assert.Equal(project.Name, addedProject.Name);
        Assert.Equal(project.Description, addedProject.Description);
    }

    [Fact]
    public void UpdateProject_ProjectUpdated()
    {
        using var context = new ICSProjectDbContext(DbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "James",
            Surname = "Adams",
            ImageUrl = null
        };

        context.Users.Add(user);
        context.SaveChanges();

        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = "Test Project",
            Description = "This is a test project"
        };

        context.Projects.Add(project);
        user.UsrProjects.Add(project);
        context.SaveChanges();

        var updatedProject = context.Projects.FirstOrDefault(p => p.Id == project.Id);
        updatedProject!.Name = "Updated Project Name";
        updatedProject.Description = "This is an updated test project";
        context.SaveChanges();

        var retrievedProject = context.Projects.FirstOrDefault(p => p.Id == updatedProject.Id);
        Assert.NotNull(retrievedProject);
        Assert.Equal(updatedProject.Name, retrievedProject.Name);
        Assert.Equal(updatedProject.Description, retrievedProject.Description);
    }

    [Fact]
    public void ReadProject_ProjectRetrieved()
    {
        using var context = new ICSProjectDbContext(DbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "James",
            Surname = "Adams",
            ImageUrl = null
        };

        context.Users.Add(user);
        context.SaveChanges();

        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = "Test Project",
            Description = "This is a test project"
        };
        context.Projects.Add(project);
        user.UsrProjects.Add(project);
        context.SaveChanges();

        var retrievedProject = context.Projects.FirstOrDefault(p => p.Id == project.Id);

        Assert.NotNull(retrievedProject);
        Assert.Equal(project.Name, retrievedProject.Name);
        Assert.Equal(project.Description, retrievedProject.Description);
    }

    [Fact]
    public void DeleteProject_ProjectDeletedFromDatabase()
    {
        using var context = new ICSProjectDbContext(DbContextOptions);

        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "James",
            Surname = "Adams",
            ImageUrl = null
        };

        context.Users.Add(user);
        context.SaveChanges();

        var project = new ProjectEntity
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Empty,
            Name = "Test Project",
            Description = "This is a test project"
        };
        context.Projects.Add(project);
        user.UsrProjects.Add(project);
        context.SaveChanges();

        var deletedProject = context.Projects.FirstOrDefault(p => p.Id == project.Id);
        context.Projects.Remove(deletedProject!);
        context.SaveChanges();

        var retrievedProject = context.Projects.FirstOrDefault(p => p.Id == deletedProject!.Id);
        Assert.Null(retrievedProject);
    }
}
