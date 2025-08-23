using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
using Resume.Domain.ViewModels.Page;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Controllers
{
    public class PortfolioController : Controller
    {
        #region Constructor
        private readonly IPortfolioService _porfolioService;

        public PortfolioController(IPortfolioService porfolioService)
        {
            _porfolioService = porfolioService;
        }
        #endregion


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {

            PortfolioPageViewModel model = new PortfolioPageViewModel() {
                Portfolios = await _porfolioService.GetAllPortfolios(cancellationToken),
                PortfolioCategories = await _porfolioService.GetAllPortfolioCategories(cancellationToken)
            };

            return View(model);
        }
    }
}
