using Microsoft.Extensions.DependencyInjection;
using Resume.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Application.Eetensions
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection CondigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CustomerFeedbackProfile>();
                cfg.AddProfile<CustomerLogoProfile>();
                cfg.AddProfile<EducationProfile>();
                cfg.AddProfile<ExperienceProfile>();
                cfg.AddProfile<InformationProfile>();
                cfg.AddProfile<PortfolioProfile>();
                cfg.AddProfile<SkillProfile>();
                cfg.AddProfile<SocialMediaProfile>();
                cfg.AddProfile<ThingToDoProfile>();
            });
            return services;
        }
    }
}
