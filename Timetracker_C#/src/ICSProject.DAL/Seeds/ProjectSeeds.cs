using System;
using System.Diagnostics;
using ICSProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.DAL.Seeds;

public static class ProjectSeeds
{
    public static readonly ProjectEntity Project1 = new()
    {
        Id = Guid.Parse("dd1eaa8e-4ef2-4cc7-8f0b-8cc151806ace"),
        Name = "Bar webpage",
        Description = "Work project for bar webpage.",
        UserId = UserSeeds.JaneCooper.Id
    };

    public static readonly ProjectEntity Project2 = new()
    {
        Id = Guid.Parse("f31667e7-a3b6-4f4d-8d78-e0d3e0e8259f"),
        Name = "Jewelry e-shop",
        Description = "School project for jewelry e-shop.",
        UserId = UserSeeds.JohnSmith.Id
    };

    public static readonly ProjectEntity Project3 = new()
    {
        Id = Guid.Parse("28db49b8-48ea-405b-a73f-e4478c7551df"),
        Name = "Stadium buffet",
        Description = "Sidejob for stadium buffet.",
        UserId = UserSeeds.ClarkeGriffin.Id

    };

    static ProjectSeeds()
    {
        Project1.ProjActivities.Add(ActivitySeeds.Work1);
        Project1.ProjActivities.Add(ActivitySeeds.Work2);
        Project1.ProjActivities.Add(ActivitySeeds.Work3);

        Project2.ProjActivities.Add(ActivitySeeds.School1);
        Project2.ProjActivities.Add(ActivitySeeds.School2);

        Project3.ProjActivities.Add(ActivitySeeds.Sidejob);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ProjectEntity>().HasData(
            Project1 with{ ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>()},
            Project2 with{ ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() },
            Project3 with{ ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() }
        );

}
