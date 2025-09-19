using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using System;
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
        private readonly ICacheService _cacheService;
        public GetListOfReservationsQueryHandler(IReservationRepository reservationRepository,ICacheService cacheService)
        {
            _reservationRepository = reservationRepository;
            _cacheService = cacheService;
        }

        public async Task<List<ReservationDate>> Handle
            (GetListOfReservationsQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "reservations:index:all";
            var cachedData = await _cacheService.GetAsync<List<ReservationDate>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var reservations = await _reservationRepository.GetListOfReservations(cancellationToken);

            await _cacheService.SetAsync(cacheKey, reservations, TimeSpan.FromMinutes(10));

            return reservations;

        }
    }
}
