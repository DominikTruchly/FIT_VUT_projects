using System;
using ICSProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.DAL.Seeds;

public static class ActivitySeeds
{

    public static readonly ActivityEntity Work1 = new()
    {
        ProjectId = ProjectSeeds.Project1.Id,
        UserId = UserSeeds.JaneCooper.Id,
        Id = Guid.Parse("d2174375-f35e-4231-901c-1c3727a3dc0b"),
        Name = "Homepage HTML",
        Type = "Work",
        StartDate = new DateTime(2023,
            02,
            23,
            6,
            00,
            00),
        EndDate = new DateTime(2023,
            02,
            23,
            11,
            59,
            59),
        Description = "HTML Code for homepage."
    };

    public static readonly ActivityEntity Work2 = new()
    {
        ProjectId = ProjectSeeds.Project1.Id,
        UserId = UserSeeds.JaneCooper.Id,
        Id = Guid.Parse("42ec5faf-26d1-4e0d-97c6-5246a85d7009"),
        Name = "Homepage CSS",
        Type = "Work",
        StartDate = new DateTime(2023,
            02,
            28,
            12,
            10,
            00),
        EndDate = new DateTime(2023,
            02,
            28,
            14,
            30,
            00),
        Description = "CSS file for homepage."
    };



    public static readonly ActivityEntity Work3 = new()
    {
        ProjectId = ProjectSeeds.Project1.Id,
        UserId = UserSeeds.ClarkeGriffin.Id,
        Id = Guid.Parse("604fde2c-408c-4333-9c0e-51c5c0ece89e"),
        Name = "Drinks Menu",
        Type = "Work",
        StartDate = new DateTime(2023,
            03,
            01,
            10,
            00,
            00),
        EndDate = new DateTime(2023,
            03,
            01,
            12,
            30,
            00),
        Description = "Create page for drinks."
    };

    public static readonly ActivityEntity School1 = new()
    {
        ProjectId = ProjectSeeds.Project2.Id,
        UserId = UserSeeds.JohnSmith.Id,
        Id = Guid.Parse("3441197b-6d35-4cda-bfae-34f773a79c16"),
        Name = "Product database",
        Type = "School",
        StartDate = new DateTime(2023,
            02,
            12,
            14,
            00,
            00),
        EndDate = new DateTime(2023,
            02,
            12,
            15,
            30,
            00),
        Description = "Create product database."
    };


    public static readonly ActivityEntity School2 = new()
    {
        ProjectId = ProjectSeeds.Project2.Id,
        UserId = UserSeeds.AndrewBlake.Id,
        Id = Guid.Parse("4c87480c-8e63-4e0d-8d60-e12f18500b74"),
        Name = "Web Design",
        Type = "School",
        StartDate = new DateTime(2023,
            02,
            12,
            09,
            00,
            00),
        EndDate = new DateTime(2023,
            02,
            12,
            11,
            00,
            00),
        Description = "Graphic web design."
    };

    public static readonly ActivityEntity Sidejob = new()
    {
        ProjectId = ProjectSeeds.Project3.Id,
        UserId = UserSeeds.ClarkeGriffin.Id,
        Id = Guid.Parse("b28c7d35-133e-4a70-a8c2-076d7bafb8b6"),
        Name = "Menu banner",
        Type = "Sidejob",
        StartDate = new DateTime(2023,
            02,
            13,
            16,
            00,
            00),
        EndDate = new DateTime(2023,
            02,
            13,
            16,
            37,
            00),
        Description = "Banner design."
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ActivityEntity>().HasData(
            Work1,
            Work2,
            Work3,
            School1,
            School2,
            Sidejob
        );

}
