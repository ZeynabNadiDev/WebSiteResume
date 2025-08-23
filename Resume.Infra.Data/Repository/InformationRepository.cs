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
    public class InformationRepository : GenericRepository<Information>, IInformationRepository
    {
        public InformationRepository(AppDbContext context) : base(context) { }

      public async Task<Information?> GetSingleAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(cancellationToken);

        }

    }
}
