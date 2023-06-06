using System;
using System.Diagnostics;
using ICSProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.Common.Tests.Seeds;
public static class UserSeeds
{
    public static readonly UserEntity EmptyUserEntity = new()
    {
        Id = default,
        Name = default!,
        Surname = default!,
        ImageUrl = default
    };

    public static readonly UserEntity UserEntity = new()
    {
        Id = Guid.Parse(input: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        Name = "Jane",
        Surname = "Cooper",
        ImageUrl = @"https://i.ibb.co/WyWJgDt/png-transparent-user-profile-computer-icons-profile-heroes-black-silhouette-thumbnail.png"
    };

    
    public static readonly UserEntity UserEntityWithNoActivities = UserEntity with { Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2A"), UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() };
    public static readonly UserEntity UserEntityUpdate = UserEntity with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F6786A"), UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() };
    public static readonly UserEntity UserEntityDelete = UserEntity with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A0A"), UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() };

    public static readonly UserEntity UserForActivitiesEntityUpdate = UserEntity with { Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF3A"), UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() };
    public static readonly UserEntity UserForActivitiesEntityDelete = UserEntity with { Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB8A"), UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() };

    public static readonly UserEntity UserEntity_ProjectUserEntity = new()
    {
        Id = Guid.Parse(input: "f52da4a7-8fc8-40e8-95e8-9a9e9c7db926"),
        Name = "Dano",
        Surname = "Drevo",
        UsrActivities = Array.Empty<ActivityEntity>(),
        UsrProjects = Array.Empty<ProjectEntity>()
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            UserEntity with { UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() },
            UserEntityWithNoActivities,
            UserEntity_ProjectUserEntity,
            UserEntityUpdate,
            UserEntityDelete,
            UserForActivitiesEntityUpdate,
            UserForActivitiesEntityDelete
        );
}
