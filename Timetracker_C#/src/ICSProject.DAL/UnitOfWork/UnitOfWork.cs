using System;
using System.Threading.Tasks;
using ICSProject.DAL.Entities;
using ICSProject.DAL.Mappers;
using ICSProject.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ICSProject.DAL.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;

    public UnitOfWork(DbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new()
        => new Repository<TEntity>(_dbContext, new TEntityMapper());

    public async Task CommitAsync() => await _dbContext.SaveChangesAsync();

    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
}
