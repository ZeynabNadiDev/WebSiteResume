using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Queries.CustomerFeedbacks;
using Resume.Application.CQRS.Queries.CustomerLogos;
using Resume.Application.CQRS.Queries.ThingIDos;
using Resume.Domain.ViewModels.Page;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Controllers
{
    public class HomeController : Controller
    {

        #region Constructor
        
        private readonly IMediator _mediator;
       


        public HomeController( IMediator mediator)
        {
           
             _mediator = mediator;
           
        }
        #endregion
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {

            IndexPageViewModel model = new IndexPageViewModel()
            {
                ThingIDoList = await _mediator.Send(new GetAllThingIDoForIndexQuery()),
                CustomerFeedbakcList = await _mediator.Send(new GetAllCustomerFeedbacksForIndexQuery()),
                CustomerLogoList = await _mediator.Send(new GetCustomerLogosForIndexPageQuery())
            };

            return View(model);

        }

    }
}
