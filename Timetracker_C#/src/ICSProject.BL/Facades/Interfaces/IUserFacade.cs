using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Facades;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel>
{
    Task UpdateAsync(UserDetailModel model);
}
