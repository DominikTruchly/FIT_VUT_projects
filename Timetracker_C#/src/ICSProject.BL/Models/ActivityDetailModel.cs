using System;

namespace ICSProject.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Type { get; set; }

    public required Guid UserId { get; set; }
    public Guid? ProjectId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string Description { get; set; }

    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        UserId = Guid.Empty,
        Name = string.Empty,
        Type = string.Empty,
        StartDate = DateTime.Today,
        EndDate = DateTime.Today,
        Description = string.Empty,
    };
}
