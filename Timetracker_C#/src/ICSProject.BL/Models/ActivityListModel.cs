using System;

namespace ICSProject.BL.Models;

public record ActivityListModel : ModelBase
{
    public required string Name { get; set; }
    public required string Type { get; set; }

    public DateTime StartDate { get; set;  }
    public DateTime EndDate { get; set; }
    public required Guid UserId { get; set; }
    public Guid? ProjectId { get; set; }
    
    public static ActivityListModel Empty => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        Name = string.Empty,
        Type = string.Empty,
        StartDate = DateTime.Today,
        EndDate = DateTime.Today
    };
}
