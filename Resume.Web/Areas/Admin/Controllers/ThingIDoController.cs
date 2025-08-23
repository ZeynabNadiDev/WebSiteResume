using Microsoft.AspNetCore.Mvc;
using Resume.Application.Services.Interfaces;
using Resume.Domain.ViewModels.ThingIDo;
using Resume.Web.Areas.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Admin.Controllers
{
    public class ThingIDoController : AdminBaseController
    {
        #region Constructor
        private readonly IThingIDoService _thingIDOService;

        public ThingIDoController(IThingIDoService thingIDOService)
        {
            _thingIDOService = thingIDOService;
        }
        #endregion


        #region List
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _thingIDOService.GetAllThingIDoForIndex(cancellationToken));
        }
        #endregion


        public async Task<IActionResult> LoadThingIDoFormModal(long id,CancellationToken cancellationToken)
        {
            CreateOrEditThingIDoViewModel result = await _thingIDOService.FillCreateOrEditThingIDoViewModel(id, cancellationToken);

            return PartialView("_ThingIDoFormModalPartial", result);
        }


        public async Task<IActionResult> SubmitThingIDoFormModal(CreateOrEditThingIDoViewModel thingIDo,CancellationToken cancellationToken)
        {
            var result = await _thingIDOService.CreateOrEditThingIDo(thingIDo, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }


        public async Task<IActionResult> DeleteThingIDO(long id,CancellationToken cancellationToken)
        {
            var result = await _thingIDOService.DeleteThingIDo(id, cancellationToken);

            if (result) return new JsonResult(new { status = "Success" });

            return new JsonResult(new { status = "Error" });
        }


    }
}
