using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Commands.PortfolioCategories;
using Resume.Application.CQRS.Queries.PortfolioCategories;
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
        private readonly IMediator _mediator;
        public PortfolioCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _mediator.Send(new GetAllPortfolioCategoriesQuery(),cancellationToken));
        }

        public async Task<IActionResult> LoadPortfolioCategoryFormModal(long id,CancellationToken cancellationToken)
        {
            CreateOrEditPortfolioCategoryViewModel result = await _mediator.Send(new FillCreateOrEditPortfolioCategoryViewModelQuery(id),cancellationToken);
            return PartialView("_PortfolioCategorFormModalPartial", result);
        }

        public async Task<IActionResult> SubmitPortfolioCategoryFormModal(CreateOrEditPortfolioCategoryViewModel portfolioCategory,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateOrEditPortfolioCategoryCommand(portfolioCategory), cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }

        public async Task<IActionResult> DeletePortfolioCategory(long id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeletePortfolioCategoryCommand(id), cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }


    }
}
