using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Convertors;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Reservation;
using System;
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
        private readonly ILogger<CreateOrEditReservationDateCommandHandler> _logger;

        public CreateOrEditReservationDateCommandHandler(
            IReservationRepository reservationRepository,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateOrEditReservationDateCommandHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditReservationDateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrEditReservationDateCommand, Id: {Id}", request.ReservationVm.Id);

            try
            {
                if (request.ReservationVm.Id == 0)
                {
                    _logger.LogInformation("Creating new ReservationDate");
                    var newEntity = _mapper.Map<ReservationDate>(request.ReservationVm);
                    newEntity.Date = request.ReservationVm.ReservationDate.ToMiladiDateTime();

                    await _reservationRepository.AddAsync(newEntity, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("ReservationDate created successfully, Id: {Id}", newEntity.Id);

                    await _cacheService.RemoveAsync("reservations:index:all");
                    _logger.LogInformation("Cache invalidated for reservations:index:all");

                    return true;
                }

                _logger.LogInformation("Editing ReservationDate, Id: {Id}", request.ReservationVm.Id);
                var currentEntity = await _reservationRepository.GetByIdAsync(request.ReservationVm.Id, cancellationToken);
                if (currentEntity == null)
                {
                    _logger.LogWarning("ReservationDate not found for Id: {Id}", request.ReservationVm.Id);
                    return false;
                }

                _mapper.Map(request.ReservationVm, currentEntity);
                currentEntity.Date = request.ReservationVm.ReservationDate.ToMiladiDateTime();

                _reservationRepository.Update(currentEntity);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ReservationDate updated successfully, Id: {Id}", request.ReservationVm.Id);

                await _cacheService.RemoveAsync($"reservation:{request.ReservationVm.Id}:entity");
                await _cacheService.RemoveAsync("reservations:index:all");
                _logger.LogInformation("Cache invalidated for reservation:{Id}:entity and reservations:index:all", request.ReservationVm.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating or editing ReservationDate, Id: {Id}", request.ReservationVm.Id);
                throw;
            }
        }
    }
}
