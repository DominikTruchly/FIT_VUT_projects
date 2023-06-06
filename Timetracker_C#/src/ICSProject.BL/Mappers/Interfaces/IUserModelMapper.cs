using ICSProject.BL.Models;
using ICSProject.DAL.Entities;

namespace ICSProject.BL.Mappers;

public interface IUserModelMapper : IModelMapper<UserEntity, UserListModel, UserDetailModel>
{
}
