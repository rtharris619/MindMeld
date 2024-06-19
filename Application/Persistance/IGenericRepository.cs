using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Persistance
{
    public interface IGenericRepository<TEntity> where TEntity : class    
    {
        Task<IEnumerable<TEntity>> GetList(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        Task<TEntity?> GetOne(
            Expression<Func<TEntity, bool>>? filter = null,
            string includeProperties = "");

        Task<TEntity?> GetByID(Guid id, string includeProperties = "");

        Task Add(TEntity entity);

        void Update(TEntity entity);

        Task Remove(Guid id);

        void Remove(TEntity entityToDelete);
    }
}
