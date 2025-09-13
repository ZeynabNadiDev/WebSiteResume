using MediatR;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Reservations
{
    public record GetListOfReservationsQuery() : IRequest<List<ReservationDate>>;

    public class GetListOfReservationsQueryHandler
        : IRequestHandler<GetListOfReservationsQuery, List<ReservationDate>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetListOfReservationsQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public Task<List<ReservationDate>> Handle(GetListOfReservationsQuery request, CancellationToken cancellationToken)
        {
            return _reservationRepository.GetListOfReservations(cancellationToken);
        }
    }
}
