using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Mappers;


public class UserModelMapper : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>,
    IUserModelMapper
{
    private readonly IActivityModelMapper _activityModelMapper;
    private readonly IProjectModelMapper _projectModelMapper;

    public UserModelMapper(IActivityModelMapper activityModelMapper, IProjectModelMapper projectModelMapper)
    {
        _activityModelMapper = activityModelMapper;
        _projectModelMapper = projectModelMapper;
    }

    public override UserListModel MapToListModel(UserEntity? entity)
        => entity is null
        ? UserListModel.Empty
        : new UserListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            ImageUrl = entity.ImageUrl
        };

    public override UserDetailModel MapToDetailModel(UserEntity? entity)
        => entity is null
        ? UserDetailModel.Empty
        : new UserDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            ImageUrl = entity.ImageUrl,
            UsrProjects = _projectModelMapper.MapToListModel(entity.UsrProjects).ToObservableCollection(),
            UsrActivities = _activityModelMapper.MapToListModel(entity.UsrActivities)
                .ToObservableCollection()
        };

    public override UserEntity MapToEntity(UserDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Surname = model.Surname,
            ImageUrl = model.ImageUrl,
        };
}
