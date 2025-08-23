using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Web.Areas.Controllers
{

    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";

            return View();
        }
    }


}
