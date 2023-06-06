using System;
using ICSProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity JaneCooper = new()
    {
        Id = Guid.Parse("a717d11f-9b82-4644-9a35-fcd8280eea16"),
        Name = "Jane",
        Surname = "Cooper",
        ImageUrl = @"https://t3.ftcdn.net/jpg/04/43/94/64/360_F_443946416_l2xXrFoIuUkItmyscOK5MNh6h0Vai3Ua.jpg"
    };

    public static readonly UserEntity JohnSmith = new()
    {
        Id = Guid.Parse("cc108748-1aa7-42be-aa8d-3baee475a169"),
        Name = "John",
        Surname = "Smith",
        ImageUrl = @"https://www.nicepng.com/png/detail/1007-10079572_man-user-default-suit-business-comments-customer-image.png"
    };

    public static readonly UserEntity AndrewBlake = new()
    {
        Id = Guid.Parse("e94e96a8-631f-48f7-a582-0d08b26a7f58"),
        Name = "Andrew",
        Surname = "Blake",
        ImageUrl = @"https://media.istockphoto.com/id/1131164548/vector/avatar-5.jpg?s=612x612&w=0&k=20&c=CK49ShLJwDxE4kiroCR42kimTuuhvuo2FH5y_6aSgEo="
    };

    public static readonly UserEntity ClarkeGriffin = new()
    {
        Id = Guid.Parse("727ac59b-aa22-41e8-97f9-f246ac61d6ef"),
        Name = "Julia",
        Surname = "Griffin",
        ImageUrl = @"https://media.istockphoto.com/id/1327592664/vector/default-avatar-photo-placeholder-icon-grey-profile-picture-business-woman.jpg?s=612x612&w=0&k=20&c=6SzxAmNr9PZtHIeVZa0l6RbcRpjTnyeno0fW9B5Y6Uk="
    };

    static UserSeeds()
    {
        JaneCooper.UsrActivities.Add(ActivitySeeds.Work1);
        JaneCooper.UsrActivities.Add(ActivitySeeds.Work2);
        JohnSmith.UsrActivities.Add(ActivitySeeds.School1);
        AndrewBlake.UsrActivities.Add(ActivitySeeds.School2);
        ClarkeGriffin.UsrActivities.Add(ActivitySeeds.Work3);
        ClarkeGriffin.UsrActivities.Add(ActivitySeeds.Sidejob);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            JaneCooper with { UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>()},
            JohnSmith with{ UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() },
            AndrewBlake with{ UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() },
            ClarkeGriffin with{ UsrActivities = Array.Empty<ActivityEntity>(), UsrProjects = Array.Empty<ProjectEntity>() }
        );
}
