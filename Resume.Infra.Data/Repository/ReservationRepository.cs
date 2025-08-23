using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Infra.Data.Repository;

public class ReservationRepository : GenericRepository<ReservationDate>, IReservationRepository
{
    public ReservationRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<List<ReservationDate>> GetListOfReservations(CancellationToken cancellationToken)
        => await _context.ReservationDates
                         .Where(p => !p.IsDelete)
                         .OrderByDescending(p => p.CreateDate)
                         .ToListAsync();

}
