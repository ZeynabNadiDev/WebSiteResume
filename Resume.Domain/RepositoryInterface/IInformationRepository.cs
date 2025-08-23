using Resume.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Domain.Repository
{
    public interface IInformationRepository:IGenericRepository<Information>
    {
         Task<Information?> GetSingleAsync(CancellationToken cancellationToken);
    }
}
