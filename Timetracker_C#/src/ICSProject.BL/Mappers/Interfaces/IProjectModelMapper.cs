using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Mappers;

public interface IProjectModelMapper : IModelMapper<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    ProjectEntity MapToEntity(ProjectDetailModel model, Guid projectID);
    ProjectEntity MapToEntity(ProjectListModel model, Guid projectID);
    ProjectListModel MapToListModel(ProjectDetailModel detailModel);
}
