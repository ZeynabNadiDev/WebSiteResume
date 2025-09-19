using AutoMapper;
using MediatR;
using Resume.Application.Convertors;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Reservation;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Reservations
{
    public record CreateOrEditReservationDateCommand(CreateOrUpdateReservationViewModel ReservationVm) : IRequest<bool>;

    public class CreateOrEditReservationDateCommandHandler
        : IRequestHandler<CreateOrEditReservationDateCommand, bool>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public CreateOrEditReservationDateCommandHandler(IReservationRepository reservationRepository,
            IMapper mapper, IUnitOfWork uow,ICacheService cacheService)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle
            (CreateOrEditReservationDateCommand request, CancellationToken cancellationToken)
        {
            if (request.ReservationVm.Id == 0)
            {
                var newEntity = _mapper.Map<ReservationDate>(request.ReservationVm);
                newEntity.Date = request.ReservationVm.ReservationDate.ToMiladiDateTime();
                await _reservationRepository.AddAsync(newEntity, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                await _cacheService.RemoveAsync("reservations:index:all");

                return true;
            }

            var currentEntity = await _reservationRepository.GetByIdAsync(request.ReservationVm.Id, cancellationToken);
            if (currentEntity == null) return false;

            _mapper.Map(request.ReservationVm, currentEntity);
            currentEntity.Date = request.ReservationVm.ReservationDate.ToMiladiDateTime();
            _reservationRepository.Update(currentEntity);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"reservation:{request.ReservationVm.Id}:entity");
            await _cacheService.RemoveAsync("reservations:index:all");

            return true;
        }
    }
}
