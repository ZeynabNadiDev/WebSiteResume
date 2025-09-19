using AutoMapper;
using MediatR;
using Resume.Application.Convertors;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Reservation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Reservations
{
    public record FillCreateOrUpdateReservationViewModelQuery(long Id) : IRequest<CreateOrUpdateReservationViewModel>;

    public class FillCreateOrUpdateReservationViewModelQueryHandler
        : IRequestHandler<FillCreateOrUpdateReservationViewModelQuery, CreateOrUpdateReservationViewModel>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FillCreateOrUpdateReservationViewModelQueryHandler(IReservationRepository reservationRepository,
            IMapper mapper,ICacheService cacheService)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _cacheService = cacheService;

        }

        public async Task<CreateOrUpdateReservationViewModel> Handle
            (FillCreateOrUpdateReservationViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrUpdateReservationViewModel { Id = 0 };
          
            var cacheKey = $"reservation:{request.Id}";
            var cacheData = await _cacheService.GetAsync<CreateOrUpdateReservationViewModel>(cacheKey);

            if (cacheData != null)
                return cacheData;

            var reservationDate = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reservationDate == null)
                return new CreateOrUpdateReservationViewModel { Id = 0 };

            var mapped = _mapper.Map<CreateOrUpdateReservationViewModel>(reservationDate);
            mapped.ReservationDate = reservationDate.Date.ToShamsi();

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }
}
