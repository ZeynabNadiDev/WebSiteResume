using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
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
        private readonly IEducationService _educationService;

        public EducationController(IEducationService educationService)
        {
            _educationService = educationService;
        }
        #endregion

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _educationService.GetAllEducations(cancellationToken));
        }

        public async Task<IActionResult> LoadEducationFormModal(long id, CancellationToken cancellationToken)
        {
            CreateOrEditEducationViewModel result = await _educationService.FillCreateOrEditEducationViewModel(id, cancellationToken);

            return PartialView("_EducationFormModalPartial", result);
        }

        public async Task<IActionResult> SubmitEducationFormModal(CreateOrEditEducationViewModel education,CancellationToken cancellationToken)
        {
            var result = await _educationService.CreateOrEditEducation(education, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

        public async Task<IActionResult> DeleteEducation(long id,CancellationToken cancellationToken)
        {
            var result = await _educationService.DeleteEducation(id, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

    }
}
