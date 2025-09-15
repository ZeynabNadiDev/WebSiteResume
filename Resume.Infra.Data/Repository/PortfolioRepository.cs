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
        private readonly ICacheService _cacheService;
        private const string PortfolioCacheKey = "portfolios:ordered";

        public PortfolioRepository(AppDbContext context, ICacheService cacheService)
            : base(context)
        {
            _cacheService = cacheService;
        }

        public async Task<List<Portfolio>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
            var cachedPortfolios = await _cacheService.GetAsync<List<Portfolio>>(PortfolioCacheKey);
            if (cachedPortfolios != null)
            {
                Console.WriteLine("📦 Portfolios from Redis cache");
                return cachedPortfolios;
            }

            var dataFromDb = await _dbSet
                .OrderBy(s => s.Order)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            await _cacheService.SetAsync(PortfolioCacheKey, dataFromDb, TimeSpan.FromHours(1));
            Console.WriteLine("💽 Portfolios from Database");
            return dataFromDb;
        }

        public override async Task AddAsync(Portfolio entity, CancellationToken cancellationToken)
        {
            await base.AddAsync(entity, cancellationToken);
            await _cacheService.RemoveAsync(PortfolioCacheKey);
        }

        public override void Update(Portfolio entity)
        {
            base.Update(entity);
            _cacheService.RemoveAsync(PortfolioCacheKey).GetAwaiter().GetResult();
        }

        public override void Delete(Portfolio entity)
        {
            base.Delete(entity);
            _cacheService.RemoveAsync(PortfolioCacheKey).GetAwaiter().GetResult();
        }
    }

    public class PortfolioCategoryRepository : GenericRepository<PortfolioCategory>, IPortfolioCategoryRepository
    {
        private readonly ICacheService _cacheService;
        private const string PortfolioCategoryCacheKey = "portfolioCategories:ordered";

        public PortfolioCategoryRepository(AppDbContext context, ICacheService cacheService)
            : base(context)
        {
            _cacheService = cacheService;
        }

        public async Task<List<PortfolioCategory>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
            var cachedCategories = await _cacheService.GetAsync<List<PortfolioCategory>>(PortfolioCategoryCacheKey);
            if (cachedCategories != null)
            {
                Console.WriteLine("📦 Portfolio Categories from Redis cache");
                return cachedCategories;
            }

            var dataFromDb = await _dbSet
                .OrderBy(s => s.Order)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            await _cacheService.SetAsync(PortfolioCategoryCacheKey, dataFromDb, TimeSpan.FromHours(1));
            Console.WriteLine("💽 Portfolio Categories from Database");
            return dataFromDb;
        }

        public override async Task AddAsync(PortfolioCategory entity, CancellationToken cancellationToken)
        {
            await base.AddAsync(entity, cancellationToken);
            await _cacheService.RemoveAsync(PortfolioCategoryCacheKey);
        }

        public override void Update(PortfolioCategory entity)
        {
            base.Update(entity);
            _cacheService.RemoveAsync(PortfolioCategoryCacheKey).GetAwaiter().GetResult();
        }

        public override void Delete(PortfolioCategory entity)
        {
            base.Delete(entity);
            _cacheService.RemoveAsync(PortfolioCategoryCacheKey).GetAwaiter().GetResult();
        }
    }
}
