using ICSProject.DAL.Entities;
using ICSProject.BL.Facades;
using ICSProject.BL.Models;

namespace ICSProject.BL.Facades;

public interface IProjectFacade : IFacade<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    Task UpdateAsync(ProjectDetailModel model);
    Task<ProjectDetailModel> SaveAsync(ProjectDetailModel model, Guid userID);

    Task<ProjectDetailModel> GetProjectDetailsAsync(Guid id);
    Task<ProjectDetailModel> GetProjByIdAsync(Guid id);
}
