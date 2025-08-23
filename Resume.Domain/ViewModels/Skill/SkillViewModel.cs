using System.ComponentModel.DataAnnotations;

namespace Resume.Domain.ViewModels.Skill
{
    public class SkillViewModel
    {
        public long Id { get; set; }


        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Display(Name = "درصد")]
        public string Percent { get; set; }


        [Display(Name = "الویت")]
        public int Order { get; set; }


    }
}
