using Resume.Domain.Entity.Reservation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Resume.Application.Convertors;
using System;
using Resume.Domain.ViewModels.Reservation;

namespace Resume.Application.Services.Interfaces;

public interface IReservationService
{
    Task<List<ReservationDate>> GetListOfReservations(
        CancellationToken cancellationToken);

    Task<bool> CreateReservation(string date,
        CancellationToken cancellationToken);

    Task<ReservationDate> GetReservationDate<TKey>(TKey reservationDateId,
        CancellationToken cancellationToken);

    Task<bool> EditReservationDate(long reservationDateId,
        string date,
        CancellationToken cancellationToken);

    Task<CreateOrUpdateReservationViewModel> FillCreateOrUpdateReservationViewModel(long id,
        CancellationToken cancellationToken);

    Task<bool> CreateOrEditReservationDate(CreateOrUpdateReservationViewModel reservationDate,
        CancellationToken cancellationToken);

    Task<bool> DeleteReservationDate(long id, 
        CancellationToken cancellationToken);
}
