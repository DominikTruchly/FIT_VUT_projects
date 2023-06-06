using System;
using ICSProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.Common.Tests.Seeds;

public static class ActivitySeeds
{
    public static readonly ActivityEntity EmptyActivity = new()
    {
        Id = default,
        UserId = default,
        ProjectId = default,
        Name = default!,
        Type = default!,
        StartDate = default!,
        EndDate = default!,
        Description = default!
    };

    public static readonly ActivityEntity Activity = new()
    {
        Id = Guid.Parse(input: "cccccccc-cccc-cccc-cccc-cccccccccccc"),
        UserId = UserSeeds.UserEntity.Id,
        ProjectId = ProjectSeeds.ProjectEntity.Id,
        Name = "Work",
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
        Description = "I am working"
    };

    
    public static readonly ActivityEntity ActivityEntityUpdate = Activity with { Id = Guid.Parse("143332B9-080E-4953-AEA5-BEF64679B052") };
    public static readonly ActivityEntity ActivityEntityDelete = Activity with { Id = Guid.Parse("274D0CC9-A948-4818-AADB-A8B4C0506619") };

    public static ActivityEntity ActivityEntity1 = new()
    {
        Id = Guid.Parse(input: "df935095-8709-4040-a2bb-b6f97cb416dc"),
        UserId = UserSeeds.UserEntity.Id,
        ProjectId = ProjectSeeds.ProjectEntity.Id,
        Name = "ActivityEntity1",
        Type = "ActivityEntity1",
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
        Description = "I am working ActivityEntity1"
    };

    public static ActivityEntity ActivityEntity2 = new()
    {
        Id = Guid.Parse(input: "23b3902d-7d4f-4213-9cf0-112348f56238"),
        UserId = UserSeeds.UserEntity.Id,
        ProjectId = ProjectSeeds.ProjectEntity.Id,
        Name = "ActivityEntity2",
        Type = "ActivityEntity2",
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
        Description = "I am working ActivityEntity2"
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ActivityEntity>().HasData(
            Activity,
            ActivityEntity1,
            ActivityEntity2,
            ActivityEntityUpdate,
            ActivityEntityDelete
        );

}
