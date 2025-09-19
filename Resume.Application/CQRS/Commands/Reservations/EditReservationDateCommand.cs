using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Convertors;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
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
        private readonly ILogger<EditReservationDateCommandHandler> _logger;

        public EditReservationDateCommandHandler(
            IReservationRepository reservationRepository,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<EditReservationDateCommandHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(EditReservationDateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling EditReservationDateCommand, Id: {Id}, New Date: {Date}", request.Id, request.Date);

            try
            {
                var originalRecord = await _reservationRepository.GetByIdAsync(request.Id, cancellationToken);
                if (originalRecord == null)
                {
                    _logger.LogWarning("ReservationDate not found for Id: {Id}", request.Id);
                    return false;
                }

                originalRecord.Date = request.Date.ToMiladiDateTime();
                _reservationRepository.Update(originalRecord);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ReservationDate edited successfully, Id: {Id}", request.Id);

                await _cacheService.RemoveAsync($"reservation:{request.Id}:entity");
                await _cacheService.RemoveAsync("reservations:index:all");
                _logger.LogInformation("Cache invalidated for reservation:{Id}:entity and reservations:index:all", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing ReservationDate, Id: {Id}", request.Id);
                throw;
            }
        }
    }
}
