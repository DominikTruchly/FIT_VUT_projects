using System;
using System.Collections.ObjectModel;

namespace ICSProject.BL.Models;

public record UserDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? ImageUrl { get; set; }

    public ObservableCollection<ActivityListModel> UsrActivities { get; set; } = new();
    public ObservableCollection<ProjectListModel> UsrProjects { get; set; } = new();

    public static UserDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Surname = string.Empty
    };
}
