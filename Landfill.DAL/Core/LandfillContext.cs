using Landfill.DAL.Implementation.EntityConfiguration;
using Landfill.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.DAL.Implementation.Core
{
    public class LandfillContext : DbContext
    {
        private readonly ILogger<LandfillContext> logger;

        public LandfillContext(DbContextOptions<LandfillContext> options, ILogger<LandfillContext> logger) : base(options) {
            this.logger = logger;
        }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<ContentTranslation> ContentTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnnouncementEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContentTranslationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FaqEntityTypeConfiguration());
            try
            {
                modelBuilder.Seed();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Database.");
            }
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ContentTranslation>(transaltion =>
            { 
                transaltion.HasOne<Content>(c => c.Content)
                .WithMany(tr => tr.Translations)
                .HasForeignKey(c => c.ContentId);
            });

        }
    }
}
