using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.Eetensions;
using Resume.Application.Generator;
using Resume.Application.Services.Interfaces;
using Resume.Application.StaticTools;
using Resume.Domain.ViewModels.CustomerFeedback;
using Resume.Web.Areas.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class CustomerFeedbackController : AdminBaseController
    {

        #region Constructor
        private readonly ICustomerFeedbackService _customerFeedbackService;

        public CustomerFeedbackController(ICustomerFeedbackService customerFeedbackService)
        {
            _customerFeedbackService = customerFeedbackService;
        }
        #endregion


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _customerFeedbackService.GetAllCustomerFeedbacksAsyncForIndex(cancellationToken));
        }

        public async Task<IActionResult> LoadCustomrFeedbackFormModal(long id,CancellationToken cancellationToken)
        {
            CreateOrEditCustomerFeedbackViewModel result = await _customerFeedbackService.FillCreateOrEditCustomerFeedbackViewModelAsync(id, cancellationToken);
            return PartialView("_CustomerFeedbackFormModalPartial", result);
        }

        public async Task<IActionResult> SubmitCustomerFeedbackFormModal(CreateOrEditCustomerFeedbackViewModel customerFeedback,CancellationToken cancellationToken)
        {
            var result = await _customerFeedbackService.CreateOrEditCustomerFeedbackAsync(customerFeedback,cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

        public async Task<IActionResult> DeleteCustomerFeedback(long id, CancellationToken cancellationToken)
        {
            var result = await _customerFeedbackService.DeleteCustomerFeedbackAsync(id, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

        [HttpPost]
        public async Task<IActionResult> UploadCustomerFeedbackImageAjax(IFormFile file)
        {
            if (file !=null)
            {
                if (Path.GetExtension(file.FileName) == ".png" || Path.GetExtension(file.FileName) == ".jpeg" || Path.GetExtension(file.FileName) == ".jpg")
                {
                    var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(file.FileName);
                    await file.AddImageAjaxToServer(imageName, FilePaths.CustomerFeedbackAvatarServer);
                    return new JsonResult(new { status = "Success", imageName = imageName });

                }
                else
                {
                    return new JsonResult(new { status = "Error" });
                }
            }
            else
            {
                return new JsonResult(new { status = "Error" });
            }
        }




    }
}
