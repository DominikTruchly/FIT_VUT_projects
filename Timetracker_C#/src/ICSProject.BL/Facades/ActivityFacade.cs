using ICSProject.BL.Mappers;
using ICSProject.BL.Models;
using ICSProject.DAL.Entities;
using ICSProject.DAL.Mappers;
using ICSProject.DAL.Repositories;
using ICSProject.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.BL.Facades;

public class ActivityFacade : FacadeBase<ActivityEntity, ActivityListModel,
    ActivityDetailModel, ActivityEntityMapper>, IActivityFacade
{
    private readonly IActivityModelMapper _modelMapper;
    private readonly ActivityEntityMapper _entityMapper;

    public ActivityFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityModelMapper modelMapper, ActivityEntityMapper entityMapper) : base(unitOfWorkFactory, modelMapper)
    {
        _modelMapper = modelMapper;
        _entityMapper = entityMapper;
    }

    public async Task AddActivityToProjectAsync(ActivityListModel model, Guid userID, Guid projectID)
    {
        ActivityEntity searchedEntity = _modelMapper.MapToEntity(model, userID);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        if (await repository.ExistsAsync(searchedEntity))
        {
            searchedEntity = await repository.UpdateAsync(searchedEntity);
            searchedEntity.ProjectId = projectID;
            await uow.CommitAsync();
        }
    }

    public async Task UpdateAsync(ActivityDetailModel model)
    {
        ActivityEntity searchedEntity = _modelMapper.MapToEntity(model, model.UserId);
        ActivityEntity updatedEntity = _modelMapper.MapToEntity(model, model.UserId);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        var activitiesToCheck = await GetAllByIdAsync(model.UserId);

        searchedEntity = await repository.UpdateAsync(searchedEntity);

        bool conflict = true;

        if (searchedEntity.StartDate == updatedEntity.StartDate && searchedEntity.EndDate == updatedEntity.EndDate)
        {
            conflict = false;
        }
        else { conflict = CheckConflicts(updatedEntity, activitiesToCheck); }

        if (conflict is not true)
        {
            if (await repository.ExistsAsync(searchedEntity))
            {
                searchedEntity = await repository.UpdateAsync(searchedEntity);
                _entityMapper.MapToExistingEntity(searchedEntity, updatedEntity);
                await uow.CommitAsync();
            }
        }
        else
        {
            throw new InvalidOperationException("Dates in conflict");
        }
    }


    public async Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, Guid userId)
    {
        ActivityEntity entity = _modelMapper.MapToEntity(model, userId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository =
            uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        var activitiesToCheck = await GetAllByIdAsync(userId);

        bool conflict = CheckConflicts(entity, activitiesToCheck);

        if (conflict is not true)
        {
            await repository.InsertAsync(entity);

            await uow.CommitAsync();

            return ModelMapper.MapToDetailModel(entity);
        }
        else
        {
            throw new InvalidOperationException("Dates in conflict");
        }
    }

    public bool CheckConflicts(ActivityEntity entity, IEnumerable<ActivityEntity> activitiesToCheck)
    {
        bool conflict = true;

        if (entity.StartDate >= entity.EndDate)
        {
            return conflict;
        }

        if (activitiesToCheck.Count() == 0)
        {
            return false;
        }

        foreach (var activity in activitiesToCheck)
        {
            if (entity.Id != activity.Id)
            {
                if (entity.EndDate < activity.StartDate)
                {
                    conflict = false;
                }
                else if (entity.StartDate > activity.EndDate)
                {
                    conflict = false;
                }
                else
                {
                    conflict = true;
                    break;
                }
            }

        }
        return conflict;
    }

    public async Task<IEnumerable<ActivityEntity>> GetByProjectIdAsync(Guid projectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.ProjectId == projectId)
            .ToListAsync();

        return entities;
    }

    

    public async Task<IEnumerable<ActivityEntity>> GetAllByIdAsync(Guid userId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        List<ActivityEntity> entities = await uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.UserId == userId)
            .ToListAsync();

        return entities;
    }

    public Task<IEnumerable<ActivityListModel>> GetFilteredActivitiesAsync(Guid userId, string firstSelected) => throw new NotImplementedException();

    public async Task <IEnumerable<ActivityListModel>> GetFilteredActivitiesAsync(UserEntity user, string select)
    {
        DateTime today;
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> activities;
        List<ActivityListModel> filterActivities;
        switch (select)
        {
            case "This day":
                today = DateTime.Today;
                activities = user.UsrActivities
                .Where(e => e.StartDate.Day == today.Day)
                .Where(e => e.StartDate.Month == today.Month)
                .Where(e => e.StartDate.Year == today.Year)
                .OrderBy(e => e.StartDate)
                .ToList();
                break;

            case "This week":
                today = DateTime.Today;
                activities = user.UsrActivities
                .Where(e => e.StartDate.Day + 7 == today.Day + 7)
                .Where(e => e.StartDate.Month == today.Month)
                .Where(e => e.StartDate.Year == today.Year)
                .OrderBy(e => e.StartDate)
                .ToList();
                break;

            case "This month":
                today = DateTime.Today;
                activities = user.UsrActivities
                .Where(e => e.StartDate.Month == today.Month)
                .Where(e => e.StartDate.Year == today.Year)
                .OrderBy(e => e.StartDate)
                .ToList();
                break;

            case "Last month":
                today = DateTime.Today;
                activities = user.UsrActivities
                .Where(e => e.StartDate.Month == today.Month - 1)
                .Where(e => e.StartDate.Year == today.Year)
                .OrderBy (e => e.StartDate)
                .ToList();
                break;
                
            case "This year":
                today = DateTime.Today;
                activities = user.UsrActivities
                .Where(e => e.StartDate.Year == today.Year)
                .OrderBy(e => e.StartDate)
                .ToList();
                break;

            default:
                return null;
        }
        filterActivities = ModelMapper.MapToListModel(activities).ToList();
        return filterActivities;
    }
}

