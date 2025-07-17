using Stakeholders.Core.UseCases;

namespace Stakeholders.Core.Domain.RepositoryInterfaces;

public interface ICrudRepository<TEntity> where TEntity : Entity
{
    PagedResult<TEntity> GetPaged(int page, int pageSize);
    TEntity Get(long id);
    TEntity Create(TEntity entity);
    TEntity Update(TEntity entity);
    void Delete(long id);
}