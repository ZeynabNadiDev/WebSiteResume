using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
using Resume.Domain.ViewModels.Page;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Controllers
{
    public class HomeController : Controller
    {

        #region Constructor
        private readonly IThingIDoService _thingIDoService;
        private readonly ICustomerFeedbackService _customerFeedbackService;
        private readonly ICustomerLogoService _customerLogoService;


        public HomeController(IThingIDoService thingIDoService, ICustomerFeedbackService customerFeedbackService, ICustomerLogoService customerLogoService)
        {
            _thingIDoService = thingIDoService;
            _customerFeedbackService = customerFeedbackService;
            _customerLogoService = customerLogoService;
        }
        #endregion
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {

            IndexPageViewModel model = new IndexPageViewModel()
            {
                ThingIDoList = await _thingIDoService.GetAllThingIDoForIndex(cancellationToken),
                CustomerFeedbakcList = await _customerFeedbackService.GetAllCustomerFeedbacksAsyncForIndex(cancellationToken),
                CustomerLogoList = await _customerLogoService.GetCustomerLogosForIndexPage(cancellationToken)
            };

            return View(model);

        }

    }
}
