using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
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
        private readonly ILogger<DeleteReservationDateCommandHandler> _logger;

        public DeleteReservationDateCommandHandler(
            IReservationRepository reservationRepository,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<DeleteReservationDateCommandHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteReservationDateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteReservationDateCommand, Id: {Id}", request.Id);

            try
            {
                var reservationDate = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
                if (reservationDate == null)
                {
                    _logger.LogWarning("ReservationDate not found for Id: {Id}", request.Id);
                    return false;
                }

                reservationDate.IsDelete = true;
                _reservationRepository.Update(reservationDate);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ReservationDate deleted successfully (soft delete), Id: {Id}", request.Id);

                await _cacheService.RemoveAsync($"reservation:{request.Id}:entity");
                await _cacheService.RemoveAsync("reservations:index:all");
                _logger.LogInformation("Cache invalidated for reservation:{Id}:entity and reservations:index:all", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting ReservationDate, Id: {Id}", request.Id);
                throw;
            }
        }
    }
}
