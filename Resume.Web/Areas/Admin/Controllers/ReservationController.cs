using Microsoft.AspNetCore.Mvc;
using MediatR;
using Resume.Domain.ViewModels.Reservation;
using Resume.Application.CQRS.Queries.Reservations;
using Resume.Application.CQRS.Commands.Reservations;
using Resume.Web.Areas.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class ReservationController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var reservations = await _mediator.Send(new GetListOfReservationsQuery(), cancellationToken);
            return View(reservations);
        }

        public async Task<IActionResult> LoadReservationFormModal(long id, CancellationToken cancellationToken = default)
        {
            var viewModel = await _mediator.Send(new FillCreateOrUpdateReservationViewModelQuery(id), cancellationToken);
            return PartialView("_ReservationFormModalPartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReservationFormModal(CreateOrUpdateReservationViewModel reservation, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new CreateOrEditReservationDateCommand(reservation), cancellationToken);

            if (result)
                return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReservation(long id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteReservationDateCommand(id), cancellationToken);

            if (result)
                return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }
    }
}
