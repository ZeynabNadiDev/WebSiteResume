using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
using Resume.Domain.ViewModels.Portfolio;
using Resume.Web.Areas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class PortfolioCategoryController : AdminBaseController
    {

        #region Constructor
        private readonly IPortfolioService _portfolioService;
        public PortfolioCategoryController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }
        #endregion

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _portfolioService.GetAllPortfolioCategories(cancellationToken));
        }

        public async Task<IActionResult> LoadPortfolioCategoryFormModal(long id,CancellationToken cancellationToken)
        {
            CreateOrEditPortfolioCategoryViewModel result = await _portfolioService.FillCreateOrEditPortfolioCategoryViewModel(id,cancellationToken);
            return PartialView("_PortfolioCategorFormModalPartial", result);
        }

        public async Task<IActionResult> SubmitPortfolioCategoryFormModal(CreateOrEditPortfolioCategoryViewModel portfolioCategory,CancellationToken cancellationToken)
        {
            var result = await _portfolioService.CreateOrEditPortfolioCategory(portfolioCategory, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

        public async Task<IActionResult> DeletePortfolioCategory(long id,CancellationToken cancellationToken)
        {
            var result = await _portfolioService.DeletePortfolioCategory(id, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }


    }
}
