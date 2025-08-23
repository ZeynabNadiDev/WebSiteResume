using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
using Resume.Web.Areas.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class MessageController : AdminBaseController
    {
        #region Constructor
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        #endregion
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _messageService.GetAllMessages(cancellationToken));
        }

        public async Task<IActionResult> DeleteMessage(long id,CancellationToken cancellationToken)
        {
            var result = await _messageService.DeleteMessage(id, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });

        }

    }
}
