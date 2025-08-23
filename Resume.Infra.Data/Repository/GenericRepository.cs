using Microsoft.EntityFrameworkCore;
using Resume.Domain.Repository;
using Resume.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Resume.Domain.Entity;

namespace Resume.Infra.Data.Repository
{
    public class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet=context.Set<TEntity>();
        }
        public async Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken)
        {
           return await _dbSet.FindAsync(new object [] {id },cancellationToken);
        }
        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
       public async Task SaveChangeAsync(CancellationToken cancellationToken)
        {
           await _context.SaveChangesAsync(cancellationToken);
        }
        public IQueryable<TEntity> GetEntities()
        {
            return _dbSet.AsQueryable();
        }
    }
}
