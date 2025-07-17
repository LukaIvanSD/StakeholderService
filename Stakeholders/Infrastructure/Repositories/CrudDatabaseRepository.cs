using Microsoft.EntityFrameworkCore;
using Stakeholders.Core.Domain;
using Stakeholders.Core.Domain.RepositoryInterfaces;
using Stakeholders.Core.UseCases;

namespace Stakeholders.Infrastructure.Repositories;

public class CrudDatabaseRepository<TEntity, TDbContext> : ICrudRepository<TEntity>
    where TEntity : Entity
    where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;
    private readonly DbSet<TEntity> _dbSet;

    public CrudDatabaseRepository(TDbContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TEntity>();
    }

    public PagedResult<TEntity> GetPaged(int page, int pageSize)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();
        
        var totalCount = query.Count();
        
        var items = query
            .OrderBy(e => EF.Property<object>(e, "Id"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        var remainingCount = Math.Max(0, totalCount - page * pageSize);

        return new PagedResult<TEntity>(items, totalCount, remainingCount);
    }

    public TEntity Get(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new KeyNotFoundException("Not found: " + id);
        return entity;
    }

    public TEntity Create(TEntity entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        try
        {
            DbContext.Update(entity);
            DbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        return entity;
    }

    public void Delete(long id)
    {
        var entity = Get(id);
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}