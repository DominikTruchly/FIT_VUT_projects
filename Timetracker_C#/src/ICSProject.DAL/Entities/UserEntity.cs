namespace ICSProject.DAL.Entities;
public record UserEntity : IEntity
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<ProjectEntity> UsrProjects { get; init; } = new List<ProjectEntity>();
        public ICollection<ActivityEntity> UsrActivities { get; init; } = new List<ActivityEntity>();

        public required Guid Id { get; set; }
    }
