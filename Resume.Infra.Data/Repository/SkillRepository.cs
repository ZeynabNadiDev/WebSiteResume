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
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        private readonly ICacheService _cacheService;
        private const string SkillCacheKey = "skills:ordered";

        public SkillRepository(AppDbContext context, ICacheService cacheService)
            : base(context)
        {
            _cacheService = cacheService;
        }

        public async Task<List<Skill>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
           
            var cachedSkills = await _cacheService.GetAsync<List<Skill>>(SkillCacheKey);
            if (cachedSkills != null)
        
                return cachedSkills;
            

            var skillsFromDb = await _dbSet
                .OrderBy(s => s.Order)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            await _cacheService.SetAsync(SkillCacheKey, skillsFromDb, TimeSpan.FromHours(1));
            Console.WriteLine("💽 Data from Database");
            return skillsFromDb;
        }

        public override async Task AddAsync(Skill entity, CancellationToken cancellationToken)
        {
            await base.AddAsync(entity, cancellationToken);
            await _cacheService.RemoveAsync(SkillCacheKey);
        }

        public override void Update(Skill entity)
        {
            base.Update(entity);
            _cacheService.RemoveAsync(SkillCacheKey).GetAwaiter().GetResult();
        }

        public override void Delete(Skill entity)
        {
            base.Delete(entity);
            _cacheService.RemoveAsync(SkillCacheKey).GetAwaiter().GetResult();
        }
    }
}