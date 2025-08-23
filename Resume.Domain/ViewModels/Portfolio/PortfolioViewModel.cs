using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.ViewModels.Portfolio
{
    public class PortfolioViewModel
    {
        public long Id { get; set; }


        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Display(Name = "تاریخ شروع")]
        public string Link { get; set; }


        [Display(Name = "تصویر")]
        public string Image { get; set; }


        [Display(Name = "توضیح تصویر")]
        public string ImageAlt { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; }


        [Display(Name = "عنوان دسته بندی")]
        public string PortfolioCategoryName { get; set; }


    }
}
