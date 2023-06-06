using System;
using System.Collections.Generic;

namespace ICSProject.DAL.Entities;

public record ProjectEntity : IEntity
{
    public required Guid UserId { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }

    public UserEntity? User { get; set; }

    public ICollection<UserEntity> ProjUsers { get; init; } = new List<UserEntity>();
    public ICollection<ActivityEntity> ProjActivities { get; set; } = new List<ActivityEntity>();

    public required Guid Id { get; set; }
} 
