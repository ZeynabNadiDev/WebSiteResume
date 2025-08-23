using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.ViewModels.Portfolio
{
    public class PortfolioCategoryViewModel
    {

        public long Id { get; set; }


        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Display(Name = "نام")]
        public string Name { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; }


    }
}
