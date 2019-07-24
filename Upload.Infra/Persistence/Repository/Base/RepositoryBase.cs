using System.Data.Entity;
using System.Linq;
using Upload.Domain.Entities.Base;
using Upload.Domain.Interfaces.Repositories.Base;

namespace Upload.Infra.Persistence.Repository.Base
{
    public class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId>
        where TEntity : EntityBase
        where TId : struct
    {
        private readonly DbContext _context;
        public RepositoryBase(DbContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> Get()
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            return query;
        }

        public TEntity GetById(TId id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity Insert(TEntity entity)
        {
            return _context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }
    }
}
