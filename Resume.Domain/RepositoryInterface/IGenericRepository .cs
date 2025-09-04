using System;
using Resume.Domain.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Resume.Domain.Entity.InterfaceEntity;

namespace Resume.Domain.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class,IEntity
    {
        Task<TEntity> GetByIdAsync<TKey>(TKey id,CancellationToken cancellationToken);
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);   
        Task AddAsync(TEntity entity,CancellationToken cancellationToken);
        void Update(TEntity entity);
        void Delete(TEntity entity);
     
        IQueryable<TEntity> GetEntities();


    }
}
