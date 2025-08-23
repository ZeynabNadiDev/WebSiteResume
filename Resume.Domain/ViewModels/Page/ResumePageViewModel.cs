using Resume.Domain.ViewModels.Education;
using Resume.Domain.ViewModels.Experience;
using Resume.Domain.ViewModels.Skill;
using System.Collections.Generic;

namespace Resume.Domain.ViewModels.Page
{
    public class ResumePageViewModel
    {
        public List<EducationViewModel> Educations { get; set; }
        public List<ExperienceViewModel> Experiences { get; set; }
        public List<SkillViewModel> Skills { get; set; }

    }
}
