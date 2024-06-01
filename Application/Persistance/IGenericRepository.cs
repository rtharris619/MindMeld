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
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        TEntity GetByID(Guid id);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(Guid id);
    }
}
