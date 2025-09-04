using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Convertors;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Entity.Reservation;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Reservation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations;

public class ReservationService : IReservationService
{
    #region Constructor ReservationRepository
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    public ReservationService(IReservationRepository reservationRepository,IMapper mapper,IUnitOfWork unit)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _uow = unit;
    }
    #endregion
    public async Task<List<ReservationDate>> GetListOfReservations(CancellationToken cancellationToken)
        => await _reservationRepository.GetListOfReservations(cancellationToken);

    public async Task<bool> CreateReservation(string date , 
        CancellationToken cancellationToken)
    {
        await _reservationRepository.AddAsync(new ReservationDate()
        {
            Date = date.ToMiladiDateTime()
        } , cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ReservationDate> GetReservationDate<TKey>(TKey reservationDateId,
        CancellationToken cancellationToken)
        => await _reservationRepository.GetByIdAsync(reservationDateId, cancellationToken);

    public async Task<bool> EditReservationDate(long reservationDateId ,
        string date ,   
        CancellationToken cancellationToken)
    {
        //Find original record 
        var originalRecord = await _reservationRepository.GetByIdAsync(reservationDateId , cancellationToken);
        if (originalRecord == null)
            return false;

        originalRecord.Date = date.ToMiladiDateTime();

        _reservationRepository.Update(originalRecord);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<CreateOrUpdateReservationViewModel> FillCreateOrUpdateReservationViewModel(long id , 
        CancellationToken cancellationToken)
    {
        if (id == 0) 
            return new CreateOrUpdateReservationViewModel() { Id = 0 };

        ReservationDate reservationDate = await GetReservationDate(id , cancellationToken);

        if (reservationDate == null) 
            return new CreateOrUpdateReservationViewModel() { Id = 0 };

        var viewModel = _mapper.Map<CreateOrUpdateReservationViewModel>(reservationDate);
        viewModel.ReservationDate = reservationDate.Date.ToShamsi();

        return viewModel;
    }

    public async Task<bool> CreateOrEditReservationDate(CreateOrUpdateReservationViewModel reservationDate ,
        CancellationToken cancellationToken)
    {
        if (reservationDate.Id == 0)
        {
            var newReservationDate = _mapper.Map<ReservationDate>(reservationDate);
            newReservationDate.Date = reservationDate.ReservationDate.ToMiladiDateTime();

            await _reservationRepository.AddAsync(newReservationDate , cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }

        ReservationDate currentReservationDate = await GetReservationDate(reservationDate.Id , cancellationToken);

        if (currentReservationDate == null) 
            return false;
        _mapper.Map(reservationDate, currentReservationDate);
        currentReservationDate.Date = reservationDate.ReservationDate.ToMiladiDateTime();

        _reservationRepository.Update(currentReservationDate);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteReservationDate(long id , CancellationToken cancellationToken)
    {
        ReservationDate reservationDate = await GetReservationDate(id , cancellationToken);
        if (reservationDate == null) 
            return false;

        reservationDate.IsDelete = true;

        _reservationRepository.Update(reservationDate);
        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}
