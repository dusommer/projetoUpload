using System.Linq;

namespace Upload.Domain.Interfaces.Repositories.Base
{
    public interface IRepositoryBase<TEntity, TId>
       where TEntity : class
       where TId : struct
    {
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
        IQueryable<TEntity> Get();
        TEntity GetById(TId id);
    }
}
