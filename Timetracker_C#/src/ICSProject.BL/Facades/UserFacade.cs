using ICSProject.BL.Mappers;
using ICSProject.BL.Models;
using ICSProject.DAL.Entities;
using ICSProject.DAL.Mappers;
using ICSProject.DAL.Repositories;
using ICSProject.DAL.UnitOfWork;

namespace ICSProject.BL.Facades;

public class UserFacade :
    FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    private readonly IUserModelMapper _modelMapper;
    private readonly UserEntityMapper _entityMapper;

    public UserFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        UserEntityMapper entityMapper,
        IUserModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _modelMapper = modelMapper;
        _entityMapper = entityMapper;
    }

    protected override string IncludesNavigationPathDetail =>
        $"{nameof(UserEntity.UsrActivities)}.{nameof(ActivityEntity.Project)}";

    public async Task UpdateAsync(UserDetailModel model)
    {
        UserEntity searchedEntity = _modelMapper.MapToEntity(model);
        UserEntity updatedEntity = _modelMapper.MapToEntity(model);
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<UserEntity> repository =
            uow.GetRepository<UserEntity, UserEntityMapper>();

        if (await repository.ExistsAsync(searchedEntity))
        {
            searchedEntity = await repository.UpdateAsync(searchedEntity);
            _entityMapper.MapToExistingEntity(searchedEntity, updatedEntity);
            await uow.CommitAsync();
        }
    }
}
