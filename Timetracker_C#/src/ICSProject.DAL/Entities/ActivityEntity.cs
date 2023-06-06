using System;

namespace ICSProject.DAL.Entities;

public record ActivityEntity : IEntity
{
    public Guid? ProjectId { get; set; }
    public required Guid UserId { get; set; }

    public ProjectEntity? Project { get; init; }
    public UserEntity? User { get; init; }

    public required string Name { get; set; }
    public required string Type { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public required string? Description { get; set; }

    public required Guid Id { get; set; }
}
