using Resume.Domain.Entity.Reservation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Resume.Domain.Repository;

public interface IReservationRepository:IGenericRepository<ReservationDate>
{
    Task<List<ReservationDate>> GetListOfReservations(
        CancellationToken cancellationToken);
    
   
}
