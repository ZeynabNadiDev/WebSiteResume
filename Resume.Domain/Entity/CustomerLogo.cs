using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.Entity
{
    public class CustomerLogo
    {
        [Key]
        public long Id { get; set; }


        [Display(Name = "لوگو")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Logo { get; set; }


        [Display(Name = "توضیحات لوگو")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string LogoAlt { get; set; }


        [Display(Name = "لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Link { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; } = 0;

    }
}
