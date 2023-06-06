using ICSProject.DAL.Entities;

namespace ICSProject.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.ProjectId = newEntity.ProjectId;
        existingEntity.UserId = newEntity.UserId;

        existingEntity.Name = newEntity.Name;
        existingEntity.Type = newEntity.Type;

        existingEntity.StartDate = newEntity.StartDate;
        existingEntity.EndDate = newEntity.EndDate;

        existingEntity.Description = newEntity.Description;

        existingEntity.Id = newEntity.Id;
    }
}
