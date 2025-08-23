using Microsoft.EntityFrameworkCore;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Infra.Data.Repository
{
    public class SocialMediaRepository:GenericRepository<SocialMedia>,ISocialMediaRepository
    {
        public SocialMediaRepository(AppDbContext context) : base(context) { }

        public async Task<List<SocialMedia>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                .OrderBy(s=>s.Order)
                .ToListAsync(cancellationToken);
        }
    }
}
