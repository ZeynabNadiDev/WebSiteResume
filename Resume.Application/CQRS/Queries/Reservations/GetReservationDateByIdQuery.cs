using MediatR;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Reservations
{
    public record GetReservationDateByIdQuery(long Id) : IRequest<ReservationDate?>;

    public class GetReservationDateByIdQueryHandler
        : IRequestHandler<GetReservationDateByIdQuery, ReservationDate?>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationDateByIdQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public Task<ReservationDate?> Handle(GetReservationDateByIdQuery request, CancellationToken cancellationToken)
        {
            return _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
