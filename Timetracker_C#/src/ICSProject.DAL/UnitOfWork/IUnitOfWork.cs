using System;
using System.Threading.Tasks;
using ICSProject.DAL.Entities;
using ICSProject.DAL.Mappers;
using ICSProject.DAL.Repositories;

namespace ICSProject.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();

    Task CommitAsync();
}
