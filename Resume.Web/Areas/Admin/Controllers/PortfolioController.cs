using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.Eetensions;
using Resume.Application.Generator;
using Resume.Application.Services.Interfaces;
using Resume.Application.StaticTools;
using Resume.Domain.ViewModels.Portfolio;
using Resume.Web.Areas.Controllers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class PortfolioController : AdminBaseController
    {
        #region Constructor
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }
        #endregion


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _portfolioService.GetAllPortfolios(cancellationToken));
        }

        public async Task<IActionResult> LoadPortfolioFormModal(long id,CancellationToken cancellationToken)
        {
            CreateOrEditPortfolioViewModel result = await _portfolioService.FillCreateOrEditPortfolioViewModel(id, cancellationToken);
            return PartialView("_PortfolioFormModalPartial", result);
        }

        public async Task<IActionResult> SubmitPortfolioFormModal(CreateOrEditPortfolioViewModel portfolio,CancellationToken cancellationToken)
        {
            var result = await _portfolioService.CreateOrEditPortfolio(portfolio, cancellationToken);
            if (result) return new JsonResult(new { status = "Success" });
            return new JsonResult(new { status = "Error" });
        }

        public async Task<IActionResult> DeletePortfolio(long id,CancellationToken cancellationToken)
        {
            var result = await _portfolioService.DeletePortfolio(id, cancellationToken);
            if (result) return new JsonResult(new { status = "Success" });
            return new JsonResult(new { status = "Error" });
        }

        [HttpPost]
        public async Task<IActionResult> UploadPortfolioImageAjax(IFormFile file)
        {
            if (file != null)
            {
                if (Path.GetExtension(file.FileName) == ".png" || Path.GetExtension(file.FileName) == ".jpeg" || Path.GetExtension(file.FileName) == ".jpg")
                {
                    var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(file.FileName);
                    await file.AddImageAjaxToServer(imageName, FilePaths.PortfolioServer);
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
