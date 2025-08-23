using Microsoft.EntityFrameworkCore;
using Resume.Domain.Entity;
using Resume.Domain.Entity.Reservation;
using System.Linq;

namespace Resume.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {

        #region Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        #endregion

        #region DbSet

        public DbSet<ThingIDo> ThingIDos { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public DbSet<CustomerLogo> CustomerLogos { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioCategory> PortfolioCategories { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Information> Information { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<ReservationDate> ReservationDates { get; set; }
        public DbSet<ReservationDateTime> ReservationDateTimes { get; set; }
        public DbSet<PersonSelectedReservation> PersonSelectedReservations { get; set; }

        #endregion

        #region On Model Cretaing
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            base.OnModelCreating(modelBuilder);
        }
        #endregion

    }
}
