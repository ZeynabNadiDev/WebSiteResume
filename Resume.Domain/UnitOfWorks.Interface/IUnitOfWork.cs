using Resume.Domain.Entity;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Domain.UnitOfWorks.Interface
{
    public interface IUnitOfWork
    {
    
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
       
    }
}
