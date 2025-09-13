using MediatR;
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

        public DeleteReservationDateCommandHandler(IReservationRepository reservationRepository, IUnitOfWork uow)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteReservationDateCommand request, CancellationToken cancellationToken)
        {
            var reservationDate = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (reservationDate == null) return false;

            reservationDate.IsDelete = true;
            _reservationRepository.Update(reservationDate);

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
