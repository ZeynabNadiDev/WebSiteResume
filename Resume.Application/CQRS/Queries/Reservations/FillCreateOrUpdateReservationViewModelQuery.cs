using AutoMapper;
using MediatR;
using Resume.Application.Convertors;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Reservation;
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

        public FillCreateOrUpdateReservationViewModelQueryHandler(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task<CreateOrUpdateReservationViewModel> Handle(FillCreateOrUpdateReservationViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrUpdateReservationViewModel { Id = 0 };

            var reservationDate = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reservationDate == null)
                return new CreateOrUpdateReservationViewModel { Id = 0 };

            var vm = _mapper.Map<CreateOrUpdateReservationViewModel>(reservationDate);
            vm.ReservationDate = reservationDate.Date.ToShamsi();
            return vm;
        }
    }
}
