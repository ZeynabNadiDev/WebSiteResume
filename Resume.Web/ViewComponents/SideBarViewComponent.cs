using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Queries.Informations;
using Resume.Application.CQRS.Queries.SocialMedias;
using Resume.Domain.ViewModels.ViewComponent;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.ViewComponents
{
    public class SideBarViewComponent : ViewComponent
    {
        #region Constructor
        private readonly  IMediator _mediator;
        

        public SideBarViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            SideBarViewModel model = new SideBarViewModel()
            {
                SocialMedias = await _mediator.Send(new GetAllSocialMediasQuery(),cancellationToken),
                information = await _mediator.Send(new GetInformationQuery(),cancellationToken)
            };

            return View("SideBar", model);
        }

    }
}
