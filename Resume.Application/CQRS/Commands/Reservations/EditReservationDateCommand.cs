using MediatR;
using Resume.Application.Convertors;
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

        public EditReservationDateCommandHandler(IReservationRepository reservationRepository, IUnitOfWork uow)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
        }

        public async Task<bool> Handle(EditReservationDateCommand request, CancellationToken cancellationToken)
        {
            var originalRecord = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (originalRecord == null) return false;

            originalRecord.Date = request.Date.ToMiladiDateTime();
            _reservationRepository.Update(originalRecord);

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
