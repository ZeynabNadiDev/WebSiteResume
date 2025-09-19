using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Reservations
{
    public record GetReservationDateByIdQuery(long Id) : IRequest<ReservationDate?>;

    public class GetReservationDateByIdQueryHandler
        : IRequestHandler<GetReservationDateByIdQuery, ReservationDate?>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICacheService _cacheService;
        public GetReservationDateByIdQueryHandler(IReservationRepository reservationRepository,ICacheService cacheService)
        {
            _reservationRepository = reservationRepository;
            _cacheService = cacheService;
        }

        public async Task<ReservationDate?> Handle
            (GetReservationDateByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"reservation:{request.Id}:entity";
            var cachedData = await _cacheService.GetAsync<ReservationDate>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var reservation = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reservation != null)
                await _cacheService.SetAsync(cacheKey, reservation, TimeSpan.FromMinutes(10));

            return reservation;
        }
    }
}
