using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Resume.Web.ViewComponents
{
    public class MetaTagViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View("MetaTag");
        }
    }
}
