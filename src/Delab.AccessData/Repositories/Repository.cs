using Delab.AccessData.Context;
using Delab.Shared.Entities;
using Delab.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Delab.AccessData.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected readonly DBContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DBContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public IQueryable<TEntity> Consult(Expression<Func<TEntity, bool>>? filter = null)
    {
        var query = DbSet.AsNoTracking().AsQueryable();

        if (filter != null) query = query.Where(filter);

        return query;
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetById(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        return entity ?? null;
    }

    public async Task<TEntity?> Create(TEntity entity)
    {
        DbSet.Add(entity);        
        return await SaveChanges() ? entity : null;
    }

    public async Task<bool> Update(TEntity entity)
    {
        DbSet.Update(entity);
        return await SaveChanges();
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = new TEntity { Id = id };
        DbSet.Remove(entity);
        return await SaveChanges();
    }

    public async Task<bool> SaveChanges()
    {
        return await Db.SaveChangesAsync() > 0;
    }    

    public void Dispose()
    {
        Db?.Dispose();
        GC.SuppressFinalize(this);
    }    
}
