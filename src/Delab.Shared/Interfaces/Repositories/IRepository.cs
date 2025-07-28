using Delab.Shared.Entities;
using System.Linq.Expressions;

namespace Delab.Shared.Interfaces.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
{
    IQueryable<TEntity> Consult(Expression<Func<TEntity, bool>>? filter = null);
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity?> GetById(Guid id);
    Task<TEntity?> Create(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> Delete(Guid id);
    Task<bool> SaveChanges();
}
