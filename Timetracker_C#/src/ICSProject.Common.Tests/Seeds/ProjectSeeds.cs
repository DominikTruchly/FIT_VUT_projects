using System;
using ICSProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.Common.Tests.Seeds;
public static class ProjectSeeds
{
    public static readonly ProjectEntity EmptyProjectEntity = new()
    {
        Id = default,
        UserId = default,
        Name = default!,
        Description = default!
    };

    public static readonly ProjectEntity ProjectEntity = new()
    {
        Id = Guid.Parse(input: "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        UserId = UserSeeds.UserEntity.Id,
        Name = "MyProject",
        Description = "My first test project"
    };

   
    public static readonly ProjectEntity ProjectEntityWithNoActivity = ProjectEntity with { Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"), ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() };
    public static readonly ProjectEntity ProjectEntityUpdate = ProjectEntity with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() };
    public static readonly ProjectEntity ProjectEntityDelete = ProjectEntity with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"), ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() };
        
    public static readonly ProjectEntity ProjectForActivityEntityUpdate = ProjectEntity with { Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"), ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() };
    public static readonly ProjectEntity ProjectForActivityEntityDelete = ProjectEntity with { Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"), ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() };

    public static readonly ProjectEntity ProjectEntity_ProjectUserEntity = new()
    {
        Name = "Turnaj",
        Description = "Makkyho Zbirku",
        Id = Guid.Parse(input: "640df19b-1f19-4aaf-9c0f-7b90c0d86b6e"),
        UserId = UserSeeds.UserEntity.Id,
        ProjActivities = Array.Empty<ActivityEntity>(),
        ProjUsers = Array.Empty<UserEntity>()
    };
    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ProjectEntity>().HasData(
            ProjectEntity with { ProjActivities = Array.Empty<ActivityEntity>(), ProjUsers = Array.Empty<UserEntity>() },
            ProjectEntityWithNoActivity,
            ProjectEntityUpdate,
            ProjectEntityDelete,
            ProjectEntity_ProjectUserEntity,
            ProjectForActivityEntityUpdate,
            ProjectForActivityEntityDelete
        );

}

