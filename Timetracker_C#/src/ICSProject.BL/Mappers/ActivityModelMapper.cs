using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Mappers;


public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>,
    IActivityModelMapper
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
        => entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                Type = entity.Type,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };

    public ActivityListModel MapToListModel(ActivityDetailModel detailModel) => new()
    {
        Id = detailModel.Id,
        UserId = detailModel.UserId,
        ProjectId = detailModel.ProjectId,
        Name = detailModel.Name,
        Type = detailModel.Type,
        StartDate = detailModel.StartDate,
        EndDate = detailModel.EndDate
    };

    

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null
        ? ActivityDetailModel.Empty
        : new ActivityDetailModel
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
            Name = entity.Name,
            Type = entity.Type,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Description = entity.Description is null ? "" : entity.Description,
        };

    public ActivityDetailModel MapToDetailModel(ActivityDetailModel detailModel) => new()
    {
        Id = detailModel.Id,
        UserId = detailModel.UserId,
        ProjectId = detailModel.ProjectId,
        Name = detailModel.Name,
        Type = detailModel.Type,
        StartDate = detailModel.StartDate,
        EndDate = detailModel.EndDate,
        Description = null
    };


    public ActivityEntity MapToEntity(ActivityDetailModel model, Guid userID)
        => new()
        {
            Id = model.Id,
            UserId = userID,
            ProjectId = model.ProjectId,
            Name = model.Name,
            Type = model.Type,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };

    public ActivityEntity MapToEntity(ActivityListModel model, Guid userID)
        => new()
        {
            Id = model.Id,
            UserId = userID,
            Name = model.Name,
            Type = model.Type,
            Description = null
        };

    public ActivityEntity MapToEntity(ActivityDetailModel model, Guid userID, Guid projectID)
        => new()
        {
            Id = model.Id,
            UserId = userID,
            ProjectId = projectID,
            Name = model.Name,
            Type = model.Type,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };

    public ActivityEntity MapToEntity(ActivityListModel model, Guid userID, Guid projectID)
        => new()
        {
            Id = model.Id,
            UserId = userID,
            ProjectId = projectID,
            Name = model.Name,
            Type = model.Type,
            Description = null
        };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
        => throw new NotImplementedException("This method is unsupported. Use the other overload.");
}
