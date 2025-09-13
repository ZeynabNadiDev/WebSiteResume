using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Commands.Educations;
using Resume.Application.CQRS.Queries.Educations;
using Resume.Domain.ViewModels.Education;
using Resume.Web.Areas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class EducationController : AdminBaseController
    {
        #region Constructor
        private readonly IMediator _mediator;

        public EducationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _mediator.Send(new GetAllEducationsQuery(), cancellationToken));
        }

        public async Task<IActionResult> LoadEducationFormModal(long id, CancellationToken cancellationToken)
        {
             var result = await _mediator.Send(new FillCreateOrEditEducationViewModelQuery(id), cancellationToken);
    return PartialView("_EducationFormModalPartial", result);
        }

        public async Task<IActionResult> SubmitEducationFormModal(CreateOrEditEducationViewModel education,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateOrEditEducationCommand(education), cancellationToken);
            return new JsonResult(new { status = result ? "Success" : "Error" });

        }

        public async Task<IActionResult> DeleteEducation(long id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteEducationCommand(id), cancellationToken);
            return new JsonResult(new { status = result ? "Success" : "Error" });
        }

    }
}
