using MediatR;
using Resume.Application.Convertors;
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

        public CreateReservationCommandHandler(IReservationRepository reservationRepository, IUnitOfWork uow)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
        }

        public async Task<bool> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            await _reservationRepository.AddAsync(new ReservationDate
            {
                Date = request.Date.ToMiladiDateTime()
            }, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
