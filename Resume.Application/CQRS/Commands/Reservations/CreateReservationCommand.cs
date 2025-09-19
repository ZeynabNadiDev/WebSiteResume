using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Convertors;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
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
        private readonly ILogger<CreateReservationCommandHandler> _logger;

        public CreateReservationCommandHandler(
            IReservationRepository reservationRepository,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateReservationCommandHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateReservationCommand with Date: {Date}", request.Date);

            try
            {
                var entity = new ReservationDate
                {
                    Date = request.Date.ToMiladiDateTime()
                };

                await _reservationRepository.AddAsync(entity, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ReservationDate created successfully with Id: {Id}", entity.Id);

                await _cacheService.RemoveAsync("reservations:index:all");
                _logger.LogInformation("Cache invalidated for reservations:index:all");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating ReservationDate for Date: {Date}", request.Date);
                throw;
            }
        }
    }
}
