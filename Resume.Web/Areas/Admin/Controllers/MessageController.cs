using MediatR;
using Microsoft.AspNetCore.Mvc;
using Resume.Application.CQRS.Commands.Messages;
using Resume.Application.CQRS.Queries.Messages;
using Resume.Web.Areas.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class MessageController : AdminBaseController
    {
        #region Constructor
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
           _mediator = mediator;
        }
        #endregion
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _mediator.Send(new GetAllMessagesQuery(),cancellationToken));
        }

        public async Task<IActionResult> DeleteMessage(long id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteMessageCommand(id), cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });

        }

    }
}
