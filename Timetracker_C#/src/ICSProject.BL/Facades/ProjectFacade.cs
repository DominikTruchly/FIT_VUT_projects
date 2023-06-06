using System.Collections.ObjectModel;
using ICSProject.BL.Mappers;
using ICSProject.BL.Models;
using ICSProject.DAL.Entities;
using ICSProject.DAL.Mappers;
using ICSProject.DAL.Repositories;
using ICSProject.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.BL.Facades;

public class ProjectFacade :
    FacadeBase<ProjectEntity, ProjectListModel, ProjectDetailModel, ProjectEntityMapper>, IProjectFacade
{
    private readonly IActivityFacade _activityFacade;
    private readonly IUserFacade _userFacade;

    private readonly IProjectModelMapper _modelMapper;
    private readonly IUserModelMapper _userModelMapper;
    private readonly ProjectEntityMapper _entityMapper;


    public ProjectFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IProjectModelMapper modelMapper,
        IActivityFacade activityFacade,
        IUserFacade userFacade,
        ProjectEntityMapper entityMapper,
        IUserModelMapper userModelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _modelMapper = modelMapper;
        _userFacade = userFacade;
        _userModelMapper = userModelMapper;
        _activityFacade = activityFacade;
        _entityMapper = entityMapper;
    }

    public async Task<ProjectDetailModel> GetProjectDetailsAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ProjectEntity> query = uow.GetRepository<ProjectEntity, ProjectEntityMapper>().Get();

        ProjectEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);

        UserDetailModel? userDetailModel = await _userFacade.GetAsync(entity.UserId);

        UserEntity projectUser = _userModelMapper.MapToEntity(userDetailModel);
        entity.User = projectUser;

        ObservableCollection<ActivityEntity> Activities = new();
        var activitiesTemp = await _activityFacade.GetByProjectIdAsync(id);

        foreach (var activity in activitiesTemp)
        {
            Activities.Add(activity);
        }
        entity.ProjActivities = Activities;
        return _modelMapper.MapToDetailModel(entity);
    }


    public async Task<ProjectDetailModel> GetProjByIdAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<ProjectEntity> query = uow.GetRepository<ProjectEntity, ProjectEntityMapper>().Get();
        ProjectEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return _modelMapper.MapToDetailModel(entity);
    }

    public async Task UpdateAsync(ProjectDetailModel model)
    {
        ProjectEntity searchedEntity = _modelMapper.MapToEntity(model, model.UserId);
        ProjectEntity updatedEntity = _modelMapper.MapToEntity(model, model.UserId);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ProjectEntity> repository =
            uow.GetRepository<ProjectEntity, ProjectEntityMapper>();

        if (await repository.ExistsAsync(searchedEntity))
        {
            searchedEntity = await repository.UpdateAsync(searchedEntity);
            _entityMapper.MapToExistingEntity(searchedEntity, updatedEntity);
            await uow.CommitAsync();
        }
    }

    public async Task<ProjectDetailModel> SaveAsync(ProjectDetailModel model, Guid userId)
    {
        ProjectEntity entity = _modelMapper.MapToEntity(model, userId);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ProjectEntity> repository =
            uow.GetRepository<ProjectEntity, ProjectEntityMapper>();
        await repository.InsertAsync(entity);
        await uow.CommitAsync();
        return ModelMapper.MapToDetailModel(entity);
    }
}
