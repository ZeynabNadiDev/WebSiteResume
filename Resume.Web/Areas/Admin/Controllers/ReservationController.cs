using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Implementations;
using Resume.Application.Services.Interfaces;
using Resume.Domain.ViewModels.Reservation;
using Resume.Web.Areas.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers;

public class ReservationController : AdminBaseController
{
    private readonly IReservationService _reservationService;
    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        => View(await _reservationService.GetListOfReservations(cancellationToken));

    public async Task<IActionResult> LoadReservationFormModal(long id , 
        CancellationToken cancellationToken = default)
    {
        CreateOrUpdateReservationViewModel result = await _reservationService.FillCreateOrUpdateReservationViewModel(id , cancellationToken);

        return PartialView("_ReservationFormModalPartial", result);
    }

    public async Task<IActionResult> SubmitReservationFormModal(CreateOrUpdateReservationViewModel Reservation , 
        CancellationToken cancellationToken = default)
    {
        var result = await _reservationService.CreateOrEditReservationDate(Reservation , cancellationToken);

        if (result) return new JsonResult(new { status = "Success" });

        return new JsonResult(new { status = "Error" });
    }

    public async Task<IActionResult> DeleteReservation(long id , 
        CancellationToken cancellationToken = default)
    {
        var result = await _reservationService.DeleteReservationDate(id , cancellationToken);

        if (result) return new JsonResult(new { status = "Success" });

        return new JsonResult(new { status = "Error" });
    }
}
