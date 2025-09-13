using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Commands.Skills;
using Resume.Application.CQRS.Queries.Skills;
using Resume.Domain.ViewModels.Skill;
using Resume.Web.Areas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class SkillController : AdminBaseController
    {

        #region Constructor
        private readonly IMediator _mediator;

        public SkillController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var skills = await _mediator.Send(new GetAllSkillsQuery(), cancellationToken);
            return View(skills);
        }

        public async Task<IActionResult> LoadSkillFormModal(long id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSkillByIdQuery(id), cancellationToken);
            return PartialView("_SkillFormModalPartial", result);
        }

        public async Task<IActionResult> SubmitSkillFormModal(CreateOrEditSkillViewModel skill,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateOrEditSkillCommand(skill), cancellationToken);
            return new JsonResult(new { status = result ? "Success" : "Error" });
        }

        public async Task<IActionResult> DeleteSkill(long id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteSkillCommand(id), cancellationToken);
            return new JsonResult(new { status = result ? "Success" : "Error" });

        }


    }
}
