using Microsoft.EntityFrameworkCore;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Infra.Data.Context;
using Resume.Application.Redis.Caching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Infra.Data.Repository
{
    public class PortfolioRepository : GenericRepository<Portfolio>, IPortfolioRepository
    {
    

        public PortfolioRepository(AppDbContext context)
            : base(context) { }
         
        
        public async Task<List<Portfolio>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
            

            var dataFromDb = await _dbSet
                .OrderBy(s => s.Order)
                .ToListAsync(cancellationToken);

            return dataFromDb;
        }

       
    }

    public class PortfolioCategoryRepository : GenericRepository<PortfolioCategory>, IPortfolioCategoryRepository
    {

        public PortfolioCategoryRepository(AppDbContext context)
            : base(context) { }

        public async Task<List<PortfolioCategory>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {

            var dataFromDb = await _dbSet
                .OrderBy(s => s.Order)
                .ToListAsync(cancellationToken);
            return dataFromDb;
        }
    }
}
