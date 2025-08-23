using Resume.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Domain.Repository
{
    public interface IEducationRepository:IGenericRepository<Education>
    {
        Task<List<Education>> GetAllOrderedAsync(CancellationToken cancellationToken);
    }
}
