using Resume.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Domain.Repository
{
    public interface IPortfolioRepository:IGenericRepository<Portfolio>
    {
        Task<List<Portfolio>> GetAllOrderedAsync(CancellationToken cancellationToken);
    }
    public interface IPortfolioCategoryRepository : IGenericRepository<PortfolioCategory>
    {
        Task<List<PortfolioCategory>> GetAllOrderedAsync(CancellationToken cancellationToken);
    }
}
