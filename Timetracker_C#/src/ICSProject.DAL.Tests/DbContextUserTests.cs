using ICSProject.DAL;
using ICSProject.DAL.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.DAL.Tests;

public class DbContextUserTests
{
    private readonly DbContextOptions<ICSProjectDbContext> DbContextOptions;

    public DbContextUserTests()
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
    public void NewUser_Add_Added()
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

        var addedUser = context.Users.Find(user.Id);
        Assert.Equal(user.Name, addedUser!.Name);
        Assert.Equal(user.Surname, addedUser.Surname);
        Assert.Equal(user.ImageUrl, addedUser.ImageUrl);
    }

    [Fact]
    public void GetUserById_ReturnsCorrectUser()
    {
        using var context = new ICSProjectDbContext(DbContextOptions);
        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            ImageUrl = null
        };

        context.Users.Add(user);
        context.SaveChanges();

        var retrievedUser = context.Users.Find(user.Id);
        Assert.Equal(user.Name, retrievedUser!.Name);
        Assert.Equal(user.Surname, retrievedUser.Surname);
        Assert.Equal(user.ImageUrl, retrievedUser.ImageUrl);
    }

    [Fact]
    public void UpdateUser_ModifiesUser()
    {
        using var context = new ICSProjectDbContext(DbContextOptions);
        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            Surname = "Doe",
            ImageUrl = null
        };

        context.Users.Add(user);
        context.SaveChanges();

        user.Name = "Janet";
        user.Surname = "Smith";
        context.SaveChanges();

        var updatedUser = context.Users.Find(user.Id);
        Assert.Equal(user.Name, updatedUser!.Name);
        Assert.Equal(user.Surname, updatedUser.Surname);
        Assert.Equal(user.ImageUrl, updatedUser.ImageUrl);
    }

    [Fact]
    public void DeleteUser_RemovesUser()
    {
        using var context = new ICSProjectDbContext(DbContextOptions);
        var user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Bob",
            Surname = "Johnson",
            ImageUrl = null
        };

        context.Users.Add(user);
        context.SaveChanges();

        context.Users.Remove(user);
        context.SaveChanges();

        var deletedUser = context.Users.Find(user.Id);
        Assert.Null(deletedUser);
    }
}
