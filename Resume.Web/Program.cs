using AutoMapper.Internal;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Resume.Application.CQRS.Queries.Reservations;
using Resume.Application.Eetensions;
using Resume.Application.Redis.Caching;
using Resume.Application.Mapping;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Infra.Data.Context;
using Resume.Infra.Data.Repository;
using Resume.Infra.Data.Services.Caching;
using Resume.Infra.Data.UnitOfWork;
using StackExchange.Redis;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Resume.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();

        #region Redis
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Redis");
            return ConnectionMultiplexer.Connect(connectionString);
        });

        builder.Services.AddScoped<ICacheService, RedisCacheService>();
        #endregion

        #region Add DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        #endregion

        #region Registration 

        // CQRS + MediatR Registration
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GetListOfReservationsQuery).Assembly);
        });

        // Repository Registration
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

        // UnitOfWork
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Google Recaptcha
        builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

        #endregion

        #region Encoder
        builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
        #endregion

        #region Mapping
        builder.Services.CondigureApplicationServices();
        #endregion

        var app = builder.Build();

        // Redis تست
        app.MapGet("/redis-test", async (IServiceProvider sp) =>
        {
            var mux = sp.GetRequiredService<IConnectionMultiplexer>();
            var db = mux.GetDatabase();
            await db.StringSetAsync("test:key", "hello from .NET");
            var value = await db.StringGetAsync("test:key");
            return Results.Ok(value.ToString());
        });

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
