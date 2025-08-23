using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
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
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }
        #endregion


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _skillService.GetAllSkillsAsync(cancellationToken));
        }

        public async Task<IActionResult> LoadSkillFormModal(long id,CancellationToken cancellationToken)
        {
            CreateOrEditSkillViewModel resutlt = await _skillService.FillCreateOrEditSkillViewModelAsync(id,cancellationToken);
            return PartialView("_SkillFormModalPartial", resutlt);
        }

        public async Task<IActionResult> SubmitSkillFormModal(CreateOrEditSkillViewModel skill,CancellationToken cancellationToken)
        {
            var result = await _skillService.CreateOrEditSkillAsync(skill,cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

        public async Task<IActionResult> DeleteSkill(long id,CancellationToken cancellationToken)
        {
            var result = await _skillService.DeleteSkillAsync(id, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });

        }


    }
}
