using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Commands.ThingIDos;
using Resume.Application.CQRS.Queries.ThingIDos;
using Resume.Domain.ViewModels.ThingIDo;
using Resume.Web.Areas.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class ThingIDoController : AdminBaseController
    {
        #region Constructor
        private readonly IMediator _mediator;

        public ThingIDoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion


        #region List
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _mediator.Send(new GetAllThingIDoForIndexQuery(),cancellationToken));
        }
        #endregion


        public async Task<IActionResult> LoadThingIDoFormModal(long id,CancellationToken cancellationToken)
        {
            CreateOrEditThingIDoViewModel result = await _mediator.Send(new FillCreateOrEditThingIDoViewModelQuery(id),cancellationToken);

            return PartialView("_ThingIDoFormModalPartial", result);
        }


        public async Task<IActionResult> SubmitThingIDoFormModal(CreateOrEditThingIDoViewModel thingIDo,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateOrEditThingIDoCommand(thingIDo), cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }


        public async Task<IActionResult> DeleteThingIDO(long id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteThingIDoCommand(id), cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }


    }
}
