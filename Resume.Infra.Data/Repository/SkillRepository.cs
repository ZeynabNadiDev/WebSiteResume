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
        

        public SkillRepository(AppDbContext context, ICacheService cacheService): base(context) { }
        

        public async Task<List<Skill>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
            var skillsFromDb = await _dbSet
                .OrderBy(s => s.Order)
                .ToListAsync(cancellationToken);

            return skillsFromDb;
         } 
    }
}