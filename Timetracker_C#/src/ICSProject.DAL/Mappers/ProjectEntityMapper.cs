using ICSProject.DAL.Entities;

namespace ICSProject.DAL.Mappers;

public class ProjectEntityMapper : IEntityMapper<ProjectEntity>
{
    public void MapToExistingEntity(ProjectEntity existingEntity, ProjectEntity newEntity)
    {
        existingEntity.UserId = newEntity.UserId;

        existingEntity.Name = newEntity.Name;
        existingEntity.Description = newEntity.Description;

        existingEntity.Id = newEntity.Id;
    }
}
