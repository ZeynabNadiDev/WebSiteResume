using AutoMapper.Internal;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resume.Application.Mapping;
using Resume.Application.Services.Implementations;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Infra.Data.Context;
using Resume.Infra.Data.Repository;
using Resume.Infra.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Resume.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();

        #region Add DbContext

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        #endregion

        #region Registration 

        //Service Registration
        
        builder.Services.AddScoped<IThingIDoService, ThingIDoService>();
        builder.Services.AddScoped<ICustomerFeedbackService, CustomerFeedbackService>();
        builder.Services.AddScoped<ICustomerLogoService, CustomerLogoService>();
        builder.Services.AddScoped<IEducationService, EducationService>();
        builder.Services.AddScoped<IExperienceService, ExperienceService>();
        builder.Services.AddScoped<ISkillService, SkillService>();
        builder.Services.AddScoped<IPortfolioService, PortfolioService>();
        builder.Services.AddScoped<ISocialMediaService, SocialMediaService>();
        builder.Services.AddScoped<IInformationService, InformationService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IReservationService, ReservationService>();

        //Repository Registration
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<ISkillRepository, SkillRepository>();
        builder.Services.AddScoped<ICustomerFeedbackRepository, CustomerFeedbackRepository>();
        builder.Services.AddScoped<ICustomerLogoRepository, CustomerLogoRepository>();
        builder.Services.AddScoped<IEducationRepository, EducationRepository>();
        builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
        builder.Services.AddScoped<IInformationRepository, InformationRepository>();
        builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        builder.Services.AddScoped<IPortfolioCategoryRepository, PortfolioCategoryRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<ISocialMediaRepository, SocialMediaRepository>();
        builder.Services.AddScoped<IThingIDoRepository, ThingIDoRepository>();
        builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

        //UnitOfWork
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        #region Google Recaptcha
        builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
        #endregion

        #endregion

        #region Encoder
        builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
        #endregion

        #region Mapping
        builder.Services.AddAutoMapper(cfg =>
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
    
        #endregion

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "area",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}