using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.ViewModels.CustomerFeedback
{
    public class CustomerFeedbackViewModel
    {

        public long Id { get; set; }


        [Display(Name = "آواتار")]
        public string Avatar { get; set; }


        [Display(Name = "نام")]
        public string Name { get; set; }


        [Display(Name = "توضیحات")]
        public string Description { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; }

    }
}
