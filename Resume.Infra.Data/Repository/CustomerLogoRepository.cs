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
    public class CustomerLogoRepository: GenericRepository<CustomerLogo>,ICustomerLogoRepository
    {
        public CustomerLogoRepository(AppDbContext context) : base(context) { }

        public async Task<List<CustomerLogo>> GetAllOrderedAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                  .OrderBy(s => s.Order)
                  .ToListAsync(cancellationToken);
        }
    }
}
