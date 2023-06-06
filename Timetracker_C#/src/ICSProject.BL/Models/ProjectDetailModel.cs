using System;
using System.Collections.ObjectModel;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Models;

public record ProjectDetailModel : ModelBase
{
    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ObservableCollection<ActivityListModel> ProjActivities { get; set; } = new();
    public ObservableCollection<UserListModel> ProjUsers { get; init; } = new();

    public static ProjectDetailModel Empty => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        Name = string.Empty,
        Description = string.Empty
    };
}
