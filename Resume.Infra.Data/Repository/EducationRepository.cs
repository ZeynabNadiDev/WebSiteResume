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
    public class EducationRepository:GenericRepository<Education>,IEducationRepository
    {
        public EducationRepository(AppDbContext context) : base(context) { }

        public async Task<List<Education>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                 .OrderBy(s => s.Order)
                 .ToListAsync(cancellationToken);
        }
    }
}
