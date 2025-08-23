using Microsoft.EntityFrameworkCore;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Skill;
using Resume.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Infra.Data.Repository
{
    public class SkillRepository:GenericRepository<Skill>,ISkillRepository
    {
        public SkillRepository(AppDbContext context) : base(context) { }

        public async Task<List<Skill>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
           return await _dbSet
                .OrderBy(s=>s.Order)
                .ToListAsync(cancellationToken);
        }
    }
}
