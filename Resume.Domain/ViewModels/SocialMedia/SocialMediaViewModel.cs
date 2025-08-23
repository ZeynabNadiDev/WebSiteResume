using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.ViewModels.SocialMedia
{
    public class SocialMediaViewModel
    {
        public long Id { get; set; }


        [Display(Name = "لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(1000, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Link { get; set; }


        [Display(Name = "آیکون")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Icon { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; }


    }
}
