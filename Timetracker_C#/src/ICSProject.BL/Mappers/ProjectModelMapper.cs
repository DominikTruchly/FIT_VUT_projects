using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Mappers;

public class ProjectModelMapper : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>,
    IProjectModelMapper
{
    private readonly IActivityModelMapper _activityModelMapper;
    private IProjectModelMapper _projectModelMapperImplementation;

    public ProjectModelMapper(IActivityModelMapper activityModelMapper)
    {
        _activityModelMapper = activityModelMapper;
    }

    public override ProjectListModel MapToListModel(ProjectEntity? entity)
        => entity is null
        ? ProjectListModel.Empty
        : new ProjectListModel
        {
            Id = entity.Id,
            Name = entity.Name
        };

    public ProjectEntity MapToEntity(ProjectDetailModel model, Guid userID)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            UserId = userID
        };

    public ProjectEntity MapToEntity(ProjectListModel model, Guid projectID) => throw new NotImplementedException();

    public ProjectListModel MapToListModel(ProjectDetailModel detailModel)
        => new()
        {
            Id = detailModel.Id,
            Name = detailModel.Name
        };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
        => entity is null
        ? ProjectDetailModel.Empty
        : new ProjectDetailModel
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Name = entity.Name,
            User = entity.User,
            Description = entity.Description,
            ProjActivities = _activityModelMapper.MapToListModel(entity.ProjActivities)
                .ToObservableCollection()
        };

    

    

    public override ProjectEntity MapToEntity(ProjectDetailModel model)
        => throw new NotImplementedException("This method is unsupported. Use the other overload.");
}
