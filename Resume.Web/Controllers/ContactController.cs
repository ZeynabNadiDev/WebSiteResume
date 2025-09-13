using GoogleReCaptcha.V3.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Commands.Messages;
using Resume.Application.CQRS.Queries.Informations;
using Resume.Domain.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Controllers
{
    public class ContactController : Controller
    {

        #region Constructor
        
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IMediator _mediator;

        public ContactController(ICaptchaValidator captchaValidator, IMediator mediator)
        {
            
            _captchaValidator = captchaValidator;
            _mediator = mediator;
        }
        #endregion


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {

            var information = await _mediator.Send(new GetInformationQuery(),cancellationToken);
            ViewData["Information"] = information;
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateMessageViewModel message,CancellationToken cancellationToken)
        {

            ViewData["Information"] = await _mediator.Send(new GetInformationQuery(),cancellationToken);

            if (!await _captchaValidator.IsCaptchaPassedAsync(message.Captcha))
            {
                ViewData["FormSubmitResult"] = false;
                return View(message);
            }

            if (!ModelState.IsValid)
            {
                return View(message);
            }

            var result = await _mediator.Send(new CreateMessageCommand(message),cancellationToken);

            if (result)
            {
                ViewData["FormSubmitResult"] = true;
            }

            return View();
        }


    }
}
