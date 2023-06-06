using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model, Guid userID);
    Task UpdateAsync(ActivityDetailModel model);

    Task AddActivityToProjectAsync(ActivityListModel model, Guid userID, Guid projectID);

    Task<IEnumerable<ActivityEntity>> GetByProjectIdAsync(Guid projectID);
    Task<IEnumerable<ActivityListModel>> GetFilteredActivitiesAsync(Guid userId, string firstSelected);
}
