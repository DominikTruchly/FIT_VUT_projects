using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Mappers;

public interface IActivityModelMapper : IModelMapper<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    ActivityEntity MapToEntity(ActivityDetailModel model, Guid userID);
    ActivityEntity MapToEntity(ActivityListModel model, Guid userID);

    ActivityEntity MapToEntity(ActivityDetailModel model, Guid userID, Guid projectID);
    ActivityEntity MapToEntity(ActivityListModel model, Guid userID, Guid projectID);

    ActivityListModel MapToListModel(ActivityDetailModel detailModel);
}
