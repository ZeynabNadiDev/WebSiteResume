using MediatR;
using Resume.Application.Convertors;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Reservations
{
    public record CreateReservationCommand(string Date) : IRequest<bool>;

    public class CreateReservationCommandHandler
        : IRequestHandler<CreateReservationCommand, bool>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;


        public CreateReservationCommandHandler(IReservationRepository reservationRepository, 
            IUnitOfWork uow,ICacheService cacheService)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            await _reservationRepository.AddAsync(new ReservationDate
            {
                Date = request.Date.ToMiladiDateTime()
            }, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync("reservations:index:all");

            return true;
        }
    }
}
