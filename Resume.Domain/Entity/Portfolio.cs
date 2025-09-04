using Resume.Domain.Entity.InterfaceEntity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.Entity
{
    public class Portfolio:IEntity
    {

        [Key]
        public long Id { get; set; }


        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }


        [Display(Name = "لینک")]
        [MaxLength(1000, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Link { get; set; }


        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Image { get; set; }


        [Display(Name = "توضیح تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string ImageAlt { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; } = 0;


        public long PortfolioCategoryId { get; set; }
        public PortfolioCategory PortfolioCategory { get; set; }

    }


    public class PortfolioCategory:IEntity
    {

        [Key]
        public long Id { get; set; }


        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }


        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; } = 0;


        ICollection<Portfolio> Portfolios { get; set; }
    }


}
