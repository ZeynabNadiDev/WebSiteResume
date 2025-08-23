using Resume.Domain.ViewModels.CustomerFeedback;
using Resume.Domain.ViewModels.CustomerLogo;
using Resume.Domain.ViewModels.ThingIDo;
using System.Collections.Generic;

namespace Resume.Domain.ViewModels.Page
{
    public class IndexPageViewModel
    {
        public List<ThingIDoListViewModel> ThingIDoList { get; set; }

        public List<CustomerFeedbackViewModel> CustomerFeedbakcList { get; set; }

        public List<CustomerLogoListViewModel> CustomerLogoList { get; set; }
    }
}
