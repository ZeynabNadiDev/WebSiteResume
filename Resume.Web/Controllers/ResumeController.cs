using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Queries.Educations;
using Resume.Application.CQRS.Queries.Experiences;
using Resume.Application.CQRS.Queries.Skills;
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
        
     
        private readonly IMediator _mediator;
        public ResumeController( IMediator mediator)
        {
         
            _mediator = mediator;
        }
        #endregion

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var skills = await _mediator.Send(new GetAllSkillsQuery(), cancellationToken);
            var educations = await _mediator.Send(new GetAllEducationsQuery(), cancellationToken);
            var experiences=await _mediator.Send(new GetAllExperiencesQuery(), cancellationToken);
            ResumePageViewModel model = new ResumePageViewModel()
            {
                Educations = educations,
                Experiences = experiences,
                Skills = skills
            };

            return View(model);
        }


    }
}
