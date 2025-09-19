using MediatR;
using Resume.Application.Convertors;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Reservations
{
    public record EditReservationDateCommand(long Id, string Date) : IRequest<bool>;

    public class EditReservationDateCommandHandler
        : IRequestHandler<EditReservationDateCommand, bool>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public EditReservationDateCommandHandler(IReservationRepository reservationRepository,
            IUnitOfWork uow,ICacheService cacheService)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(EditReservationDateCommand request, CancellationToken cancellationToken)
        {
            var originalRecord = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (originalRecord == null) return false;

            originalRecord.Date = request.Date.ToMiladiDateTime();
            _reservationRepository.Update(originalRecord);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"reservation:{request.Id}:entity");
            await _cacheService.RemoveAsync("reservations:index:all");

            return true;
        }
    }
}
