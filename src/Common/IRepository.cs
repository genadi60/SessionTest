using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.Common
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> All();

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        void DeleteRange(ICollection<TEntity> entities);

        Task<int> SaveChangesAsync();

        void Update(TEntity entity);

        void UpdateRange(ICollection<TEntity> entities);
    }
}
