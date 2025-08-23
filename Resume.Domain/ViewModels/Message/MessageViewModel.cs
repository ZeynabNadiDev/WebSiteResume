using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.ViewModels.Message
{
    public class MessageViewModel
    {

        public long Id { get; set; }


        [Display(Name = " نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }


        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(250, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل وارد کنید")]
        public string Email { get; set; }


        [Display(Name = "پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(1000, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Text { get; set; }

    }
}
