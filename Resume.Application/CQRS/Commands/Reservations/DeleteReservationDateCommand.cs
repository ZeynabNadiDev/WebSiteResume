using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Reservations
{
    public record DeleteReservationDateCommand(long Id) : IRequest<bool>;

    public class DeleteReservationDateCommandHandler
        : IRequestHandler<DeleteReservationDateCommand, bool>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;


        public DeleteReservationDateCommandHandler(IReservationRepository reservationRepository,
            IUnitOfWork uow,ICacheService cacheService)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteReservationDateCommand request, CancellationToken cancellationToken)
        {
            var reservationDate = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reservationDate == null) return false;

            reservationDate.IsDelete = true;
            _reservationRepository.Update(reservationDate);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"reservation:{request.Id}:entity");
            await _cacheService.RemoveAsync("reservations:index:all");

            return true;
        }
    }
}
