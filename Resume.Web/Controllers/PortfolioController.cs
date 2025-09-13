using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Queries.PortfolioCategories;
using Resume.Application.CQRS.Queries.Portfolios;
using Resume.Domain.ViewModels.Page;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Controllers
{
    public class PortfolioController : Controller
    {
        #region Constructor
        private readonly IMediator _mediator;

        public PortfolioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {


          var  portfolios = await _mediator.Send(new GetAllPortfoliosQuery(), cancellationToken);
          var  portfolioCategories = await _mediator.Send(new GetAllPortfolioCategoriesQuery(), cancellationToken);
            var model = new PortfolioPageViewModel
            {
                Portfolios = portfolios,
                PortfolioCategories = portfolioCategories
            };

            return View(model);
        }
    }
}
