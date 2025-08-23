using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
using Resume.Domain.ViewModels.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Controllers
{
    public class ResumeController : Controller
    {

        #region Constructor
        private readonly IEducationService _educationService;
        private readonly IExperienceService _experienceService;
        private readonly ISkillService _skillService;
        public ResumeController(IEducationService educationService, IExperienceService experienceService, ISkillService skillService)
        {
            _educationService = educationService;
            _experienceService = experienceService;
            _skillService = skillService;
        }
        #endregion

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            ResumePageViewModel model = new ResumePageViewModel()
            {
                Educations = await _educationService.GetAllEducations(cancellationToken),
                Experiences = await _experienceService.GetAllExperiences(cancellationToken),
                Skills = await _skillService.GetAllSkillsAsync(cancellationToken)
            };

            return View(model);
        }


    }
}
